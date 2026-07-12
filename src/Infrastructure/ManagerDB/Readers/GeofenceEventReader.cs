// Copyright (c) 2026 Sergio Hernandez. All rights reserved.
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

namespace TrackHub.Geofencing.Infrastructure.ManagerDB.Readers;

public sealed class GeofenceEventReader(IApplicationDbContext context) : IGeofenceEventReader
{
    public async Task<IReadOnlyCollection<GeofenceEventVm>> GetOpenEventsForTransporterAsync(
        Guid transporterId,
        CancellationToken cancellationToken)
    {
        var query = from evt in context.GeofenceEvents
                    where evt.TransporterId == transporterId && evt.DepartureTimestamp == null
                    select new GeofenceEventVm(
                        evt.GeofenceEventId,
                        evt.TransporterId,
                        evt.GeofenceId,
                        evt.EventDateTime,
                        evt.DepartureTimestamp,
                        evt.Latitude,
                        evt.Longitude);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<GeofenceEventReportVm>> GetGeofenceEventsAsync(
        Guid accountId,
        Guid userId,
        DateTimeOffset fromDate,
        DateTimeOffset toDate,
        Guid? transporterId,
        CancellationToken cancellationToken)
    {
        var rawEvents = await (from evt in context.GeofenceEvents
                              join g in context.Geofences on evt.GeofenceId equals g.GeofenceId
                              join t in context.Transporters on evt.TransporterId equals t.TransporterId
                              where g.AccountId == accountId && g.Active
                              where t.UserId == userId
                              where evt.EventDateTime >= fromDate && evt.EventDateTime <= toDate
                              where transporterId == null || evt.TransporterId == transporterId
                              orderby evt.EventDateTime descending
                              select new
                              {
                                  TransporterName = t.Name,
                                  GeofenceName = g.Name,
                                  evt.EventDateTime,
                                  evt.DepartureTimestamp,
                                  evt.Latitude,
                                  evt.Longitude
                              }).ToListAsync(cancellationToken);

        return [.. rawEvents.Select(e => new GeofenceEventReportVm(
            e.TransporterName,
            e.GeofenceName,
            e.EventDateTime,
            e.DepartureTimestamp,
            FormatTotalTime(e.EventDateTime, e.DepartureTimestamp),
            e.Latitude,
            e.Longitude))];
    }

    private static string FormatTotalTime(DateTimeOffset dateTimeIn, DateTimeOffset? dateTimeOut)
    {
        if (dateTimeOut == null) return string.Empty;
        var duration = dateTimeOut.Value - dateTimeIn;
        return $"{(int)duration.TotalHours:D2}:{duration.Minutes:D2}:{duration.Seconds:D2}";
    }
}
