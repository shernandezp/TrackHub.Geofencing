namespace TrackHub.Manager.Application.Geofences.Commands.Delete;

[Authorize(Resource = Resources.Geofences, Action = Actions.Delete)]
public record DeleteGeofenceCommand(Guid Id) : IRequest;

public class DeleteGeofenceCommandHandler(IGeofenceWriter writer) : IRequestHandler<DeleteGeofenceCommand>
{
    public async Task Handle(DeleteGeofenceCommand request, CancellationToken cancellationToken)
        => await writer.DeleteGeofenceAsync(request.Id, cancellationToken);

}
