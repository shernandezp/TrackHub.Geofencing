using TrackHub.Manager.Application.GeofenceEvents.Commands.ProcessPositions;
using TrackHub.Manager.Application.GeofenceEvents.Services.Interfaces;
using TrackHub.Manager.Domain.Records;

namespace TrackHub.Manager.Application.UnitTests.GeofenceEvents.Commands.ProcessPositions;

[TestFixture]
public class ProcessPositionsCommandHandlerTests
{
    private Mock<IGeofenceDetectionService> _detectionServiceMock = null!;

    [SetUp]
    public void SetUp()
    {
        _detectionServiceMock = new Mock<IGeofenceDetectionService>();
    }

    [Test]
    public async Task Handle_ShouldCallDetectionServiceAndReturnResult()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var transporterId = Guid.NewGuid();
        var positions = new[] { new TransporterPositionDto(transporterId, 1.0, 2.0, DateTimeOffset.UtcNow) };
        var expected = new GeofenceProcessingResultVm(positions.Length, 1, 0);

        _detectionServiceMock
            .Setup(s => s.ProcessPositionsAsync(It.IsAny<IEnumerable<TransporterPositionDto>>(), accountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var handler = new ProcessPositionsCommandHandler(_detectionServiceMock.Object);
        var command = new ProcessPositionsCommand(accountId, positions);
        var cts = new CancellationTokenSource();

        // Act
        var result = await handler.Handle(command, cts.Token);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
        _detectionServiceMock.Verify(s => s.ProcessPositionsAsync(
            It.Is<IEnumerable<TransporterPositionDto>>(p => p != null && p.SequenceEqual(positions)),
            accountId,
            It.Is<CancellationToken>(ct => ct == cts.Token)), Times.Once);
    }
}
