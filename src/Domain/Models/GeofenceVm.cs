namespace TrackHub.Manager.Domain.Models;

public readonly record struct GeofenceVm(
    Guid GeofenceId,
    Guid AccountId,
    MultiPolygonVm Geom,
    string Name,
    string? Description,
    short Color,
    short Type,
    bool Active
    );
