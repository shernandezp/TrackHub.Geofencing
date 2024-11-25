namespace TrackHub.Manager.Infrastructure.ManagerDB.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Geofence> Geofences { get; set; }
    DbSet<VwTransporterPosition> Transporters { get; set; }
    DbSet<VwUser> Users { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
