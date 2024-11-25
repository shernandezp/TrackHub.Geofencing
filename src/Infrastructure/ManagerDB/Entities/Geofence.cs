using Common.Infrastructure;
using NetTopologySuite.Geometries;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Entities;

public class Geofence(Guid geofenceId, Polygon geom, Guid accountId, string name, string? description, short color, short type, bool active) : BaseAuditableEntity
{
    public Guid GeofenceId { get; set; } = geofenceId;
    public Guid AccountId { get; set; } = accountId;
    public Polygon Geom { get; set; } = geom;
    public string Name { get; set; } = name;
    public string? Description { get; set; } = description;
    public short Color { get; set; } = color;
    public short Type { get; set; } = type;
    public bool Active { get; set; } = active;

}
