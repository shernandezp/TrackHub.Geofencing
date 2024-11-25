namespace TrackHub.Manager.Infrastructure.ManagerDB.Entities;

public class VwUser(
    Guid userId,
    string username,
    Guid accountId
    )
{

    public Guid UserId { get; set; } = userId;
    public string Username { get; set; } = username;
    public Guid AccountId { get; set; } = accountId;

}
