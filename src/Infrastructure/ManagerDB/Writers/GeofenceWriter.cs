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

using System.Text.Json;
using Common.Application.Interfaces;
using NetTopologySuite.Geometries;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Writers;

public sealed class GeofenceWriter(IApplicationDbContext context, IUser user) : IGeofenceWriter
{
    public async Task<GeofenceVm> CreateGeofenceAsync(GeofenceDto geofenceDto, Guid accountId, CancellationToken cancellationToken)
    {
        var geofence = new Geofence(
            geofenceDto.GeofenceId,
            CastGeofence(geofenceDto.Geom),
            accountId,
            geofenceDto.Name,
            geofenceDto.Description,
            geofenceDto.Color,
            geofenceDto.Type,
            true);

        await context.Geofences.AddAsync(geofence, cancellationToken);
        AddAuditEvent(accountId, "CreateGeofence", geofence.GeofenceId, null, ToAuditJson(geofence));
        await context.SaveChangesAsync(cancellationToken);

        return new GeofenceVm(
            geofence.GeofenceId,
            geofence.AccountId,
            geofenceDto.Geom,
            geofence.Name,
            geofence.Description,
            geofence.Color,
            geofence.Type,
            geofence.Active);
    }

    public async Task UpdateGeofenceAsync(GeofenceDto geofenceDto, CancellationToken cancellationToken)
    {
        var geofence = await context.Geofences.FindAsync([geofenceDto.GeofenceId], cancellationToken)
            ?? throw new NotFoundException(nameof(Geofence), $"{geofenceDto.GeofenceId}");
        var oldValues = ToAuditJson(geofence);

        context.Geofences.Attach(geofence);

        geofence.Geom = CastGeofence(geofenceDto.Geom);
        geofence.Name = geofenceDto.Name;
        geofence.Description = geofenceDto.Description;
        geofence.Color = geofenceDto.Color;
        geofence.Type = geofenceDto.Type;
        geofence.Active = geofenceDto.Active;
        AddAuditEvent(geofence.AccountId, "UpdateGeofence", geofence.GeofenceId, oldValues, ToAuditJson(geofence));

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteGeofenceAsync(Guid geofenceId, CancellationToken cancellationToken)
    {
        var geofence = await context.Geofences.FindAsync([geofenceId], cancellationToken)
            ?? throw new NotFoundException(nameof(Geofence), $"{geofenceId}");

        context.Geofences.Attach(geofence);
        AddAuditEvent(geofence.AccountId, "DeleteGeofence", geofence.GeofenceId, ToAuditJson(geofence), null);

        context.Geofences.Remove(geofence);
        await context.SaveChangesAsync(cancellationToken);
    }

    private static Polygon CastGeofence(MultiPolygonVm polygon)
    {
        var coordinates = polygon.Coordinates.Select(x => new Coordinate(x.Longitude, x.Latitude)).ToArray();
        var ring = new LinearRing(coordinates);
        return new Polygon(ring)
        {
            SRID = 4326
        };
    }

    private void AddAuditEvent(Guid accountId, string action, Guid geofenceId, string? oldValuesJson, string? newValuesJson)
    {
        context.AuditEvents.Add(new AuditEvent(
            accountId,
            user.PrincipalType.ToString(),
            user.UserId?.ToString() ?? user.ClientId ?? user.SubjectId ?? string.Empty,
            action,
            "Geofence",
            geofenceId.ToString(),
            "Success",
            oldValuesJson,
            newValuesJson,
            null,
            null,
            null,
            user.CorrelationId));
    }

    private static string ToAuditJson(Geofence geofence)
        => JsonSerializer.Serialize(new
        {
            geofence.GeofenceId,
            geofence.AccountId,
            geofence.Name,
            geofence.Description,
            geofence.Color,
            geofence.Type,
            geofence.Active
        });
}
