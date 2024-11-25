using Common.Application.Interfaces;

namespace TrackHub.Manager.Application.Geofences.Commands.Create;

[Authorize(Resource = Resources.Geofences, Action = Actions.Write)]
public readonly record struct CreateGeofenceCommand(GeofenceDto Geofence) : IRequest<GeofenceVm>;

public class CreateGeofenceCommandHandler(IGeofenceWriter writer, IUserReader userReader, IUser user) : IRequestHandler<CreateGeofenceCommand, GeofenceVm>
{
    private Guid UserId { get; } = user.Id is null ? throw new UnauthorizedAccessException() : new Guid(user.Id);
    public async Task<GeofenceVm> Handle(CreateGeofenceCommand request, CancellationToken cancellationToken)
    {
        var user = await userReader.GetUserAsync(UserId, cancellationToken);
        return await writer.CreateGeofenceAsync(request.Geofence, user.AccountId, cancellationToken); 
    }
}
