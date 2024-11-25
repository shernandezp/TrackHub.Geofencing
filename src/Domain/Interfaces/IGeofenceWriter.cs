using TrackHub.Manager.Domain.Records;

namespace TrackHub.Manager.Domain.Interfaces;

public interface IGeofenceWriter
{
    Task<GeofenceVm> CreateGeofenceAsync(GeofenceDto geofenceDto, Guid accountId, CancellationToken cancellationToken);
    Task DeleteGeofenceAsync(Guid geofenceId, CancellationToken cancellationToken);
    Task UpdateGeofenceAsync(GeofenceDto geofenceDto, CancellationToken cancellationToken);
}
