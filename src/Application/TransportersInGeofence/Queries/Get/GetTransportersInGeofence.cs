using Common.Application.Interfaces;

namespace TrackHub.Manager.Application.TransportersInGeofence.Queries.Get;

[Authorize(Resource = Resources.Geofencing, Action = Actions.Read)]
public readonly record struct GetTransportersInGeofenceQuery() : IRequest<IReadOnlyCollection<TransporterInGeofenceVm>>;

public class GetTransportersInGeofenceQueryHandler(ITransportersInGeofence reader, IUserReader userReader, IUser user) : IRequestHandler<GetTransportersInGeofenceQuery, IReadOnlyCollection<TransporterInGeofenceVm>>
{
    private Guid UserId { get; } = user.Id is null ? throw new UnauthorizedAccessException() : new Guid(user.Id);

    public async Task<IReadOnlyCollection<TransporterInGeofenceVm>> Handle(GetTransportersInGeofenceQuery request, CancellationToken cancellationToken)
    { 
        var user = await userReader.GetUserAsync(UserId, cancellationToken);
        return await reader.GetTransportersInGeofencesAsync(user.AccountId, UserId, cancellationToken);
    }

}
