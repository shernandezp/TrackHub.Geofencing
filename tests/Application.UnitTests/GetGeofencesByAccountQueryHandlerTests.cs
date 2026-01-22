using TrackHub.Manager.Application.Geofences.Queries.GetByAccount;
using TrackHub.Manager.Domain.Interfaces;
using Common.Application.Interfaces;

namespace TrackHub.Manager.Application.UnitTests.Geofences.Queries.GetByAccount;

[TestFixture]
public class GetGeofencesByAccountQueryHandlerTests
{
    private Mock<IGeofenceReader> _readerMock = null!;
    private Mock<IUserReader> _userReaderMock = null!;
    private Mock<IUser> _userMock = null!;

    [SetUp]
    public void SetUp()
    {
        _readerMock = new Mock<IGeofenceReader>();
        _userReaderMock = new Mock<IUserReader>();
        _userMock = new Mock<IUser>();
        _userMock.Setup(u => u.Id).Returns(Guid.NewGuid().ToString());
    }

    [Test]
    public async Task Handle_ShouldReturnGeofencesForUserAccount()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var geofences = new[] { new GeofenceVm(Guid.NewGuid(), accountId, new MultiPolygonVm([], 4326), "name", null, 1, 1, true) };
        _userReaderMock.Setup(r => r.GetUserAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new UserVm { AccountId = accountId });
        _readerMock.Setup(r => r.GetGeofencesAsync(accountId, It.IsAny<CancellationToken>())).ReturnsAsync(geofences);

        var handler = new GetGeofencesByAccountQueryHandler(_readerMock.Object, _userReaderMock.Object, _userMock.Object);
        var query = new GetGeofencesByAccountQuery(true);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.EqualTo(geofences));
        _userReaderMock.Verify(r => r.GetUserAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        _readerMock.Verify(r => r.GetGeofencesAsync(accountId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void Constructor_ShouldThrowUnauthorizedAccessException_WhenUserIdIsNull()
    {
        // Arrange
        _userMock.Setup(u => u.Id).Returns(() => null);

        // Act & Assert
        Assert.Throws<UnauthorizedAccessException>(() => new GetGeofencesByAccountQueryHandler(_readerMock.Object, _userReaderMock.Object, _userMock.Object));
    }
}
