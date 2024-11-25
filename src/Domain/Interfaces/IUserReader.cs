namespace TrackHub.Manager.Domain.Interfaces;

public interface IUserReader
{
    Task<UserVm> GetUserAsync(Guid id, CancellationToken cancellationToken);
}
