using System.Reflection;

namespace TrackHub.Manager.Infrastructure.ManagerDB;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<Geofence> Geofences { get; set; }
    public DbSet<VwTransporterPosition> Transporters { get; set; }
    public DbSet<VwUser> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}
