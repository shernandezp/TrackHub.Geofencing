using TrackHub.Manager.Application.Geofences.Queries.Get;
using TrackHub.Manager.Application.Geofences.Queries.GetByAccount;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<GeofenceVm> GetGeofence([Service] ISender sender, [AsParameters] GetGeofenceQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<GeofenceVm>> GetGeofencesByAccount([Service] ISender sender)
        => await sender.Send(new GetGeofencesByAccountQuery());

}
