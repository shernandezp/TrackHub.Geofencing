namespace TrackHub.Manager.Domain.Interfaces;

public interface ITransportersInGeofence
{
    Task<IReadOnlyCollection<TransporterInGeofenceVm>> GetTransportersInGeofencesAsync(Guid accountId, Guid userId, CancellationToken cancellationToken);
}
