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
            points.Add(new CoordinateVm(coordinate.X, coordinate.Y));
        }
        return new MultiPolygonVm(points, 4326);
    }
}
