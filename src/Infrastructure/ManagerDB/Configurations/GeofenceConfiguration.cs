using Common.Domain.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Configurations;
public class GeofenceConfiguration : IEntityTypeConfiguration<Geofence>
{
    public void Configure(EntityTypeBuilder<Geofence> builder)
    {
        //Table name
        builder.ToTable(name: TableMetadata.Geofence, schema: SchemaMetadata.Geofencing);

        //Column names
        builder.Property(x => x.GeofenceId).HasColumnName("id");
        builder.Property(x => x.AccountId).HasColumnName("accountid");
        builder.Property(e => e.Geom)
            .HasColumnName("geom")
            .HasColumnType("geometry (Polygon, 4326)");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.Description).HasColumnName("description");
        builder.Property(x => x.Color).HasColumnName("color");
        builder.Property(x => x.Type).HasColumnName("type");
        builder.Property(x => x.Active).HasColumnName("active");

        builder.Property(t => t.Name)
            .HasMaxLength(ColumnMetadata.DefaultNameLength)
            .IsRequired();

        builder.Property(t => t.Description)
            .HasMaxLength(ColumnMetadata.DefaultDescriptionLength);

        builder
            .HasIndex(e => e.Geom)
            .HasDatabaseName("geofence_idx")
            .HasAnnotation("Npgsql:IndexMethod", "gist");
    }
}
