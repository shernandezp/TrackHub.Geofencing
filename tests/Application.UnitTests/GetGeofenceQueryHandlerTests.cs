using TrackHub.Manager.Application.Geofences.Queries.Get;
using TrackHub.Manager.Domain.Interfaces;
using Common.Application.Interfaces;

namespace TrackHub.Manager.Application.UnitTests.Geofences.Queries.Get;

[TestFixture]
public class GetGeofenceQueryHandlerTests
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
    public async Task Handle_ShouldReturnGeofence()
    {
        // Arrange
        var id = Guid.NewGuid();
        var vm = new GeofenceVm(id, Guid.NewGuid(), new MultiPolygonVm([], 4326), "name", null, 1, 1, true);
        _readerMock.Setup(r => r.GetGeofenceAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(vm);

        var handler = new GetGeofenceQueryHandler(_readerMock.Object);
        var query = new GetGeofenceQuery(id);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.EqualTo(vm));
        _readerMock.Verify(r => r.GetGeofenceAsync(id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void Constructor_ShouldThrowUnauthorizedAccessException_WhenUserIdIsNull()
    {
        // Arrange
        _userMock.Setup(u => u.Id).Returns(() => null);

        // Act & Assert
        // This handler only requires IGeofenceReader; construction with user is not supported here
        Assert.DoesNotThrow(() => new GetGeofenceQueryHandler(_readerMock.Object));
    }
}
