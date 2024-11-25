namespace TrackHub.Manager.Domain.Models;

public readonly record struct MultiPolygonVm(
    IEnumerable<CoordinateVm> Coordinates,
    int SRID
    );

