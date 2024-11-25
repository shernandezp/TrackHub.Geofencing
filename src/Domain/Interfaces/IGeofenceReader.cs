namespace TrackHub.Manager.Domain.Interfaces;

public interface IGeofenceReader
{
    Task<GeofenceVm> GetGeofenceAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<GeofenceVm>> GetGeofencesAsync(Guid accountId, CancellationToken cancellationToken);
}
