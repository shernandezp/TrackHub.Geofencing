using Common.Domain.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Configurations;
public class VwUserConfiguration : IEntityTypeConfiguration<VwUser>
{
    public void Configure(EntityTypeBuilder<VwUser> builder)
    {
        //Table name
        builder.ToView(name: ViewMetadata.VwUsers, schema: SchemaMetadata.Geofencing);

        //Column names
        builder.Property(x => x.UserId).HasColumnName("id");
        builder.Property(x => x.Username).HasColumnName("username");
        builder.Property(x => x.AccountId).HasColumnName("accountid");

        builder.HasKey(e => e.UserId);
    }
}
