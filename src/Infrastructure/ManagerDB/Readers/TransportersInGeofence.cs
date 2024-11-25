namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

public sealed class TransportersInGeofence(IApplicationDbContext context) : ITransportersInGeofence
{

    public async Task<IReadOnlyCollection<TransporterInGeofenceVm>> GetTransportersInGeofencesAsync(Guid accountId, Guid userId, CancellationToken cancellationToken)
    {
        var query = from geofence in context.Geofences
                    from transporter in context.Transporters
                    where geofence.AccountId == accountId && geofence.Active && transporter.UserId == userId
                    where geofence.Geom.Intersects(transporter.Geom)
                    select new TransporterInGeofenceVm
                    {
                        GeofenceId = geofence.GeofenceId,
                        GeofenceName = geofence.Name,
                        TransporterId = transporter.TransporterId,
                        TransporterName = transporter.Name
                    };

        return await query.ToListAsync(cancellationToken);
    }

}
