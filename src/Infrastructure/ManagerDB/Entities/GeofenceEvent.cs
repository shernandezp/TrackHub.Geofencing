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

namespace TrackHub.Manager.Infrastructure.ManagerDB.Entities;

/// <summary>
/// Entity representing a geofence entry or exit event.
/// </summary>
public class GeofenceEvent(
    Guid transporterId,
    Guid geofenceId,
    DateTime dateTime,
    TimeSpan offset,
    double latitude,
    double longitude)
{
    public Guid GeofenceEventId { get; private set; } = Guid.NewGuid();
    public Guid TransporterId { get; set; } = transporterId;
    public Guid GeofenceId { get; set; } = geofenceId;
    public DateTime DateTime { get; set; } = dateTime;
    public TimeSpan Offset { get; set; } = offset;
    /// <summary>
    /// Exit timestamp (null if still inside geofence).
    /// </summary>
    public DateTime? DepartureTimestamp { get; set; }
    public TimeSpan? DepartureOffset { get; set; }
    /// <summary>
    /// Entry location.
    /// </summary>
    public double Latitude { get; set; } = latitude;
    public double Longitude { get; set; } = longitude;
    // Navigation properties
    public Geofence? Geofence { get; set; }
}
