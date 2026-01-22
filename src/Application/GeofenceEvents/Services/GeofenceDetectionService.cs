// Copyright (c) 2025 Sergio Hernandez. All rights reserved.
//
//  Licensed under the Apache License, Version 2.0 (the "License").
//  You may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//

using TrackHub.Manager.Application.GeofenceEvents.Services.Interfaces;

namespace TrackHub.Manager.Application.GeofenceEvents.Services;

/// <summary>
/// Service that processes transporter positions to detect geofence entry/exit events.
/// </summary>
public class GeofenceDetectionService(
    IGeofenceReader geofenceReader,
    IGeofenceEventReader geofenceEventReader,
    IGeofenceEventWriter geofenceEventWriter) : IGeofenceDetectionService
{
    private static readonly TimeSpan MinEventInterval = TimeSpan.FromSeconds(30);

    public async Task<GeofenceProcessingResultVm> ProcessPositionsAsync(
        IEnumerable<TransporterPositionDto> positions,
        Guid accountId,
        CancellationToken cancellationToken)
    {
        var positionsList = positions
            .OrderBy(p => p.TransporterId)
            .ThenBy(p => p.DeviceDateTime)
            .ToList();

        if (positionsList.Count == 0)
            return new GeofenceProcessingResultVm(0, 0, 0);

        // Pre-load open events for all transporters
        var transporterIds = positionsList.Select(p => p.TransporterId).Distinct();
        var openEventsByTransporter = new Dictionary<Guid, Dictionary<Guid, GeofenceEventVm>>();

        foreach (var transporterId in transporterIds)
        {
            var openEvents = await geofenceEventReader.GetOpenEventsForTransporterAsync(transporterId, cancellationToken);
            openEventsByTransporter[transporterId] = openEvents.ToDictionary(e => e.GeofenceId);
        }

        // Process positions and accumulate results
        var eventsCreated = 0;
        var eventsUpdated = 0;

        foreach (var transporterPositions in positionsList.GroupBy(p => p.TransporterId))
        {
            var transporterId = transporterPositions.Key;
            var openEvents = openEventsByTransporter[transporterId];

            foreach (var position in transporterPositions)
            {
                var (created, updated, events) = await ProcessPositionAsync(
                    position, accountId, openEvents, cancellationToken);

                eventsCreated += created;
                eventsUpdated += updated;
            }
        }

        return new GeofenceProcessingResultVm(
            positionsList.Count,
            eventsCreated,
            eventsUpdated);
    }

    private async Task<(int Created, int Updated, List<GeofenceEventVm> Events)> ProcessPositionAsync(
        TransporterPositionDto position,
        Guid accountId,
        Dictionary<Guid, GeofenceEventVm> openEvents,
        CancellationToken cancellationToken)
    {
        var created = 0;
        var updated = 0;
        var events = new List<GeofenceEventVm>();

        // Use spatial query to find geofences containing this point (uses PostGIS ST_Contains)
        var containingGeofenceIds = await geofenceReader.GetGeofenceIdsContainingPointAsync(
            accountId, position.Latitude, position.Longitude, cancellationToken);
        var containingGeofences = containingGeofenceIds.ToHashSet();

        // Process entries (in geofence but no open event)
        foreach (var geofenceId in containingGeofences)
        {
            if (openEvents.ContainsKey(geofenceId))
                continue;

            // Debounce check
            if (openEvents.TryGetValue(geofenceId, out var recentEvent) &&
                recentEvent.DepartureTimestamp.HasValue &&
                (position.DeviceDateTime - recentEvent.DepartureTimestamp.Value) < MinEventInterval)
            {
                continue;
            }

            // Create entry event
            var newEvent = await geofenceEventWriter.CreateEntryEventAsync(
                new GeofenceEventDto(
                    position.TransporterId,
                    geofenceId,
                    position.DeviceDateTime,
                    position.Latitude,
                    position.Longitude),
                cancellationToken);

            created++;
            events.Add(newEvent);
            openEvents[geofenceId] = newEvent;
        }

        // Process exits (has open event but not in geofence)
        var geofencesToClose = openEvents.Keys
            .Where(id => !containingGeofences.Contains(id))
            .ToList();

        foreach (var geofenceId in geofencesToClose)
        {
            var openEvent = openEvents[geofenceId];

            // Debounce check
            if ((position.DeviceDateTime - openEvent.Timestamp) < MinEventInterval)
                continue;

            await geofenceEventWriter.UpdateExitEventAsync(
                openEvent.GeofenceEventId,
                position.DeviceDateTime,
                cancellationToken);

            updated++;
            openEvents.Remove(geofenceId);
        }

        return (created, updated, events);
    }
}
