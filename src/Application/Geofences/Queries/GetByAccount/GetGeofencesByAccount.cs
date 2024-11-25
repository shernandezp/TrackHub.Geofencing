using Common.Application.Interfaces;

namespace TrackHub.Manager.Application.Geofences.Queries.GetByAccount;

[Authorize(Resource = Resources.Geofences, Action = Actions.Read)]
public readonly record struct GetGeofencesByAccountQuery() : IRequest<IReadOnlyCollection<GeofenceVm>>;

public class GetGeofencesByAccountQueryHandler(IGeofenceReader reader, IUserReader userReader, IUser user) : IRequestHandler<GetGeofencesByAccountQuery, IReadOnlyCollection<GeofenceVm>>
{
    private Guid UserId { get; } = user.Id is null ? throw new UnauthorizedAccessException() : new Guid(user.Id);
    public async Task<IReadOnlyCollection<GeofenceVm>> Handle(GetGeofencesByAccountQuery request, CancellationToken cancellationToken)
    {
        var user = await userReader.GetUserAsync(UserId, cancellationToken);
        return await reader.GetGeofencesAsync(user.AccountId, cancellationToken);
    }

}
