using TrackHub.Manager.Application.TransportersInGeofence.Queries.Get;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{

    public async Task<IReadOnlyCollection<TransporterInGeofenceVm>> GetTransportersInGeofence([Service] ISender sender)
        => await sender.Send(new GetTransportersInGeofenceQuery());

}
