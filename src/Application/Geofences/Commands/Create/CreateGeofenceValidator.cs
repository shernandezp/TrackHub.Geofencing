namespace TrackHub.Manager.Application.Geofences.Commands.Create;

public sealed class CreateGeofenceValidator : AbstractValidator<CreateGeofenceCommand>
{
    public CreateGeofenceValidator()
    {
        RuleFor(v => v.Geofence)
            .NotEmpty();

        RuleFor(v => v.Geofence.Name)
            .NotEmpty();

        RuleFor(v => v.Geofence.Geom)
            .NotEmpty();

        RuleFor(v => v.Geofence.Geom.Coordinates)
            .NotEmpty();
    }
}
