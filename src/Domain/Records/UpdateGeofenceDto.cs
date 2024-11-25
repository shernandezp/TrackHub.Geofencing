namespace TrackHub.Manager.Domain.Records;

public readonly record struct GeofenceDto(
    Guid GeofenceId,
    MultiPolygonVm Geom,
    string Name,
    string? Description,
    short Color,
    short Type,
    bool Active
    );
