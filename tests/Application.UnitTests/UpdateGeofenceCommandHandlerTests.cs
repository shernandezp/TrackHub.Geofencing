using TrackHub.Manager.Application.Geofences.Commands.Update;
using TrackHub.Manager.Domain.Interfaces;
using TrackHub.Manager.Domain.Records;

namespace TrackHub.Manager.Application.UnitTests.Geofences.Commands.Update;

[TestFixture]
public class UpdateGeofenceCommandHandlerTests
{
    private Mock<IGeofenceWriter> _writerMock = null!;

    [SetUp]
    public void SetUp()
    {
        _writerMock = new Mock<IGeofenceWriter>();
    }

    [Test]
    public async Task Handle_ShouldCallUpdateOnWriter()
    {
        // Arrange
        var dto = new GeofenceDto(Guid.NewGuid(), default!, "name", null, 1, 1, true);
        _writerMock.Setup(w => w.UpdateGeofenceAsync(dto, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var handler = new UpdateGeofenceCommandHandler(_writerMock.Object);
        var command = new UpdateGeofenceCommand(dto);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _writerMock.Verify(w => w.UpdateGeofenceAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
    }
}
