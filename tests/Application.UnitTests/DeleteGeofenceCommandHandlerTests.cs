using TrackHub.Manager.Application.Geofences.Commands.Delete;
using TrackHub.Manager.Domain.Interfaces;

namespace TrackHub.Manager.Application.UnitTests.Geofences.Commands.Delete;

[TestFixture]
public class DeleteGeofenceCommandHandlerTests
{
    private Mock<IGeofenceWriter> _writerMock = null!;

    [SetUp]
    public void SetUp()
    {
        _writerMock = new Mock<IGeofenceWriter>();
    }

    [Test]
    public async Task Handle_ShouldCallDeleteOnWriter()
    {
        // Arrange
        var id = Guid.NewGuid();
        _writerMock.Setup(w => w.DeleteGeofenceAsync(id, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var handler = new DeleteGeofenceCommandHandler(_writerMock.Object);
        var command = new DeleteGeofenceCommand(id);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _writerMock.Verify(w => w.DeleteGeofenceAsync(id, It.IsAny<CancellationToken>()), Times.Once);
    }
}
