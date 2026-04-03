using TrackHub.Manager.Application.Geofences.Queries.Get;
using TrackHub.Manager.Domain.Interfaces;
using TrackHub.Manager.Domain.Models;
using Common.Application.Interfaces;
using Common.Application.Exceptions;

namespace TrackHub.Manager.Application.UnitTests.Geofences.Queries.Get;

[TestFixture]
public class GetGeofenceQueryHandlerTests
{
    private Mock<IGeofenceReader> _readerMock = null!;
    private Mock<IUserReader> _userReaderMock = null!;
    private Mock<IUser> _userMock = null!;
    private Guid _userId;
    private Guid _accountId;

    [SetUp]
    public void SetUp()
    {
        _readerMock = new Mock<IGeofenceReader>();
        _userReaderMock = new Mock<IUserReader>();
        _userMock = new Mock<IUser>();
        _userId = Guid.NewGuid();
        _accountId = Guid.NewGuid();
        _userMock.Setup(u => u.Id).Returns(_userId.ToString());
        _userReaderMock.Setup(r => r.GetUserAsync(_userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UserVm(_userId, "testuser", _accountId));
    }

    [Test]
    public async Task Handle_ShouldReturnGeofence_WhenAccountMatches()
    {
        // Arrange
        var id = Guid.NewGuid();
        var vm = new GeofenceVm(id, _accountId, new MultiPolygonVm([], 4326), "name", null, 1, 1, true);
        _readerMock.Setup(r => r.GetGeofenceAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(vm);

        var handler = new GetGeofenceQueryHandler(_readerMock.Object, _userReaderMock.Object, _userMock.Object);
        var query = new GetGeofenceQuery(id);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.EqualTo(vm));
        _readerMock.Verify(r => r.GetGeofenceAsync(id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void Handle_ShouldThrowForbiddenAccessException_WhenAccountDoesNotMatch()
    {
        // Arrange
        var id = Guid.NewGuid();
        var differentAccountId = Guid.NewGuid();
        var vm = new GeofenceVm(id, differentAccountId, new MultiPolygonVm([], 4326), "name", null, 1, 1, true);
        _readerMock.Setup(r => r.GetGeofenceAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(vm);

        var handler = new GetGeofenceQueryHandler(_readerMock.Object, _userReaderMock.Object, _userMock.Object);
        var query = new GetGeofenceQuery(id);

        // Act & Assert
        Assert.ThrowsAsync<ForbiddenAccessException>(() => handler.Handle(query, CancellationToken.None));
    }

    [Test]
    public void Constructor_ShouldThrowUnauthorizedAccessException_WhenUserIdIsNull()
    {
        // Arrange
        _userMock.Setup(u => u.Id).Returns(() => null);

        // Act & Assert
        Assert.Throws<UnauthorizedAccessException>(() =>
            new GetGeofenceQueryHandler(_readerMock.Object, _userReaderMock.Object, _userMock.Object));
    }
}
