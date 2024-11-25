namespace TrackHub.Manager.Application.Geofences.Commands.Update;

[Authorize(Resource = Resources.Geofences, Action = Actions.Edit)]
public readonly record struct UpdateGeofenceCommand(GeofenceDto Geofence) : IRequest;

public class UpdateGeofenceCommandHandler(IGeofenceWriter writer) : IRequestHandler<UpdateGeofenceCommand>
{
    public async Task Handle(UpdateGeofenceCommand request, CancellationToken cancellationToken)
        => await writer.UpdateGeofenceAsync(request.Geofence, cancellationToken);
}
