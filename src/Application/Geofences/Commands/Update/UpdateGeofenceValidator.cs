using TrackHub.Manager.Application.Geofences.Commands.Update;

namespace TrackHub.Manager.Application.Transporters.Commands.Update;

public sealed class UpdateGeofenceValidator : AbstractValidator<UpdateGeofenceCommand>
{
    public UpdateGeofenceValidator()
    {
        RuleFor(v => v.Geofence)
            .NotEmpty();

        RuleFor(v => v.Geofence.GeofenceId)
            .NotEmpty();

        RuleFor(v => v.Geofence.Name)
            .NotEmpty();

        RuleFor(v => v.Geofence.Geom)
            .NotEmpty();

        RuleFor(v => v.Geofence.Geom.Coordinates)
            .NotEmpty();
    }
}
