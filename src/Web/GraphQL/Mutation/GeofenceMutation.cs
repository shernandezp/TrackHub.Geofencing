using TrackHub.Manager.Application.Geofences.Commands.Create;
using TrackHub.Manager.Application.Geofences.Commands.Delete;
using TrackHub.Manager.Application.Geofences.Commands.Update;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<GeofenceVm> CreateGeofence([Service] ISender sender, CreateGeofenceCommand command)
        => await sender.Send(command);

    public async Task<bool> UpdateGeofence([Service] ISender sender, Guid id, UpdateGeofenceCommand command)
    {
        if (id != command.Geofence.GeofenceId) return false;
        await sender.Send(command);
        return true;
    }

    public async Task<Guid> DeleteGeofence([Service] ISender sender, Guid id)
    {
        await sender.Send(new DeleteGeofenceCommand(id));
        return id;
    }
}
