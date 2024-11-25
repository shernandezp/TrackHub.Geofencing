namespace TrackHub.Manager.Application.Geofences.Queries.Get;

public class GetGeofenceValidator : AbstractValidator<GetGeofenceQuery>
{
    public GetGeofenceValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
