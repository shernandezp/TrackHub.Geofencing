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

using NetTopologySuite.Geometries;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

public sealed class GeofenceReader(IApplicationDbContext context) : IGeofenceReader
{

    public async Task<GeofenceVm> GetGeofenceAsync(Guid id, CancellationToken cancellationToken)
    {
        var geofence = await context.Geofences
            .Where(a => a.GeofenceId.Equals(id))
            .FirstAsync(cancellationToken);

        return new GeofenceVm(geofence.GeofenceId,
            geofence.AccountId,
            CastGeofence(geofence.Geom),
            geofence.Name,
            geofence.Description,
            geofence.Color,
            geofence.Type,
            geofence.Active);
    }

    public async Task<IReadOnlyCollection<GeofenceVm>> GetGeofencesAsync(Guid accountId, CancellationToken cancellationToken)
    {
        var geofences = await context.Geofences
            .Where(a => a.AccountId.Equals(accountId))
            .ToListAsync(cancellationToken);

        return geofences.Select(geofence => new GeofenceVm(geofence.GeofenceId,
            geofence.AccountId,
            CastGeofence(geofence.Geom),
            geofence.Name,
            geofence.Description,
            geofence.Color,
            geofence.Type,
            geofence.Active))
            .ToList();
    }

    private static MultiPolygonVm CastGeofence(Polygon polygon)
    {
        var points = new List<CoordinateVm>();
        foreach (var coordinate in polygon.Coordinates)
        {
            points.Add(new CoordinateVm(coordinate.Y, coordinate.X));
        }
        return new MultiPolygonVm(points, 4326);
    }
}
