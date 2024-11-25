namespace TrackHub.Manager.Application.Geofences.Commands.Delete;

public sealed class DeleteGeofenceValidator : AbstractValidator<DeleteGeofenceCommand>
{
    public DeleteGeofenceValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty();
    }
}
