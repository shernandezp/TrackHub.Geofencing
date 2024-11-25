using Common.Domain.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Configurations;
public sealed class VmTransporterPositionConfiguration : IEntityTypeConfiguration<VwTransporterPosition>
{
    public void Configure(EntityTypeBuilder<VwTransporterPosition> builder)
    {
        //Table name
        builder.ToView(name: ViewMetadata.VwTransporterPosition, schema: SchemaMetadata.Geofencing);

        //Column names
        builder.Property(x => x.TransporterId).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(e => e.Geom)
            .HasColumnName("geom")
            .HasColumnType("geometry(Point, 4326)");
        builder.Property(x => x.UserId).HasColumnName("userid");

        //Indexes
        builder.HasKey(e => e.TransporterId);
    }
}
