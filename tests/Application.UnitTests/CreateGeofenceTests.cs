using Common.Application.Interfaces;
using TrackHub.Manager.Application.Geofences.Commands.Create;
using TrackHub.Manager.Domain.Interfaces;
using TrackHub.Manager.Domain.Records;

namespace TrackHub.Manager.Application.UnitTests.Credentials.Command.Create;

[TestFixture]
public class CreateGeofenceCommandHandlerTests
{
    private readonly Mock<IGeofenceWriter> _geofenceWriterMock = new();
    private readonly Mock<IUserReader> _userReaderMock = new();
    private readonly Mock<IUser> _userMock = new();

    [SetUp]
    public void SetUp()
    {
        _userMock.Setup(u => u.Id).Returns(Guid.NewGuid().ToString());
    }

    [Test]
    public async Task Handle_ShouldCreateGeofence()
    {
        // Arrange
        var geofenceDto = new GeofenceDto();
        var command = new CreateGeofenceCommand(geofenceDto);
        var userVm = new UserVm { AccountId = Guid.NewGuid() };
        var geofenceVm = new GeofenceVm();
        var handler = new CreateGeofenceCommandHandler(_geofenceWriterMock.Object, _userReaderMock.Object, _userMock.Object);

        _userReaderMock.Setup(r => r.GetUserAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(userVm);
        _geofenceWriterMock.Setup(w => w.CreateGeofenceAsync(It.IsAny<GeofenceDto>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(geofenceVm);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        geofenceVm.Should().BeEquivalentTo(result);
        _userReaderMock.Verify(r => r.GetUserAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        _geofenceWriterMock.Verify(w => w.CreateGeofenceAsync(It.IsAny<GeofenceDto>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void Constructor_ShouldThrowUnauthorizedAccessException_WhenUserIdIsNull()
    {
        // Arrange
        _userMock.Setup(u => u.Id).Returns(() => null);

        // Act & Assert
        Assert.Throws<UnauthorizedAccessException>(() => new CreateGeofenceCommandHandler(_geofenceWriterMock.Object, _userReaderMock.Object, _userMock.Object));
    }
}
