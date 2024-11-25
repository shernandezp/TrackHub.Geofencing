using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Npgsql;
using TrackHub.Manager.Infrastructure.ManagerDB;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString, o => 
            { 
                o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                o.UseNetTopologySuite();
            });
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        services.AddHeaderPropagation(o => o.Headers.Add("Authorization"));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IGeofenceWriter, GeofenceWriter>();
        services.AddScoped<IGeofenceReader, GeofenceReader>();
        services.AddScoped<IUserReader, UserReader>();
        services.AddScoped<ITransportersInGeofence, TransportersInGeofence>();

        return services;
    }
}
