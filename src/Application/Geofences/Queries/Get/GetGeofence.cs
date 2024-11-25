namespace TrackHub.Manager.Application.Geofences.Queries.Get;

[Authorize(Resource = Resources.Geofences, Action = Actions.Read)]
public readonly record struct GetGeofenceQuery(Guid Id) : IRequest<GeofenceVm>;

public class GetGeofenceQueryHandler(IGeofenceReader reader) : IRequestHandler<GetGeofenceQuery, GeofenceVm>
{
    public async Task<GeofenceVm> Handle(GetGeofenceQuery request, CancellationToken cancellationToken)
        => await reader.GetGeofenceAsync(request.Id, cancellationToken);

}
