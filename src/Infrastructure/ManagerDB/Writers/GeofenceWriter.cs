using NetTopologySuite.Geometries;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Writers;

public sealed class GeofenceWriter(IApplicationDbContext context) : IGeofenceWriter
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
        var geofence = await context.Geofences.FindAsync(geofenceDto.GeofenceId, cancellationToken)
            ?? throw new NotFoundException(nameof(Geofence), $"{geofenceDto.GeofenceId}");

        context.Geofences.Attach(geofence);

        geofence.Geom = CastGeofence(geofenceDto.Geom);
        geofence.Name = geofenceDto.Name;
        geofence.Description = geofenceDto.Description;
        geofence.Color = geofenceDto.Color;
        geofence.Type = geofenceDto.Type;
        geofence.Active = geofenceDto.Active;

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteGeofenceAsync(Guid geofenceId, CancellationToken cancellationToken)
    {
        var geofence = await context.Geofences.FindAsync(geofenceId, cancellationToken)
            ?? throw new NotFoundException(nameof(Geofence), $"{geofenceId}");

        context.Geofences.Attach(geofence);

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
}
