using NetTopologySuite.Geometries;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Entities;

public class VwTransporterPosition(
    Guid transporterId,
    string name,
    Point geom,
    Guid userId
    )
{

    public Guid TransporterId { get; set; } = transporterId;
    public string Name { get; set; } = name;
    public Point Geom { get; set; } = geom;
    public Guid UserId { get; set; } = userId;

}
