using TrackHub.Manager.Application.GeofenceEvents.Services;
using TrackHub.Manager.Domain.Interfaces;
using TrackHub.Manager.Domain.Records;

namespace TrackHub.Manager.Application.UnitTests.GeofenceEvents.Services;

[TestFixture]
public class GeofenceDetectionServiceTests
{
    private Mock<IGeofenceReader> _geofenceReaderMock = null!;
    private Mock<IGeofenceEventReader> _geofenceEventReaderMock = null!;
    private Mock<IGeofenceEventWriter> _geofenceEventWriterMock = null!;

    [SetUp]
    public void SetUp()
    {
        // Create fresh mocks per test to ensure isolation
        _geofenceReaderMock = new Mock<IGeofenceReader>();
        _geofenceEventReaderMock = new Mock<IGeofenceEventReader>();
        _geofenceEventWriterMock = new Mock<IGeofenceEventWriter>();
    }

    [Test]
    public async Task ProcessPositionsAsync_NoPositions_ReturnsZeros()
    {
        // Arrange
        var service = new GeofenceDetectionService(
            _geofenceReaderMock.Object,
            _geofenceEventReaderMock.Object,
            _geofenceEventWriterMock.Object);

        // Act
        var result = await service.ProcessPositionsAsync([], Guid.NewGuid(), CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.ProcessedCount, Is.Zero);
            Assert.That(result.EventsCreated, Is.Zero);
            Assert.That(result.EventsUpdated, Is.Zero);
        }
    }

    [Test]
    public async Task ProcessPositionsAsync_SinglePosition_EntersGeofence_CreatesEvent()
    {
        // Arrange
        var transporterId = Guid.NewGuid();
        var geofenceId = Guid.NewGuid();
        var timestamp = DateTimeOffset.UtcNow;

        var position = new TransporterPositionDto(transporterId, 10.0, 20.0, timestamp);

        _geofenceEventReaderMock.Setup(r => r.GetOpenEventsForTransporterAsync(transporterId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        _geofenceReaderMock.Setup(r => r.GetGeofenceIdsContainingPointAsync(It.IsAny<Guid>(), position.Latitude, position.Longitude, It.IsAny<CancellationToken>()))
            .ReturnsAsync([geofenceId]);

        var createdEvent = new GeofenceEventVm(Guid.NewGuid(), transporterId, geofenceId, timestamp, null, position.Latitude, position.Longitude);
        _geofenceEventWriterMock.Setup(w => w.CreateEntryEventAsync(It.IsAny<GeofenceEventDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdEvent);

        var service = new GeofenceDetectionService(
            _geofenceReaderMock.Object,
            _geofenceEventReaderMock.Object,
            _geofenceEventWriterMock.Object);

        // Act
        var result = await service.ProcessPositionsAsync([position], Guid.NewGuid(), CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.ProcessedCount, Is.EqualTo(1));
            Assert.That(result.EventsCreated, Is.EqualTo(1));
            Assert.That(result.EventsUpdated, Is.Zero);
        }
        _geofenceEventWriterMock.Verify(w => w.CreateEntryEventAsync(It.IsAny<GeofenceEventDto>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task ProcessPositionsAsync_EntryThenExit_UpdatesEvent()
    {
        // Arrange
        var transporterId = Guid.NewGuid();
        var geofenceId = Guid.NewGuid();
        var t0 = DateTimeOffset.UtcNow;
        var t1 = t0.AddSeconds(60);

        var posEntry = new TransporterPositionDto(transporterId, 10.0, 20.0, t0);
        var posExit = new TransporterPositionDto(transporterId, 11.0, 21.0, t1);

        // No open events initially
        _geofenceEventReaderMock.Setup(r => r.GetOpenEventsForTransporterAsync(transporterId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // First call returns the geofence id, second call returns empty
        _geofenceReaderMock.SetupSequence(r => r.GetGeofenceIdsContainingPointAsync(It.IsAny<Guid>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([geofenceId])
            .ReturnsAsync([]);

        var createdEventId = Guid.NewGuid();
        var createdEvent = new GeofenceEventVm(createdEventId, transporterId, geofenceId, t0, null, posEntry.Latitude, posEntry.Longitude);
        _geofenceEventWriterMock.Setup(w => w.CreateEntryEventAsync(It.IsAny<GeofenceEventDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdEvent);

        _geofenceEventWriterMock.Setup(w => w.UpdateExitEventAsync(createdEventId, t1, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var service = new GeofenceDetectionService(
            _geofenceReaderMock.Object,
            _geofenceEventReaderMock.Object,
            _geofenceEventWriterMock.Object);

        // Act
        var result = await service.ProcessPositionsAsync([posEntry, posExit], Guid.NewGuid(), CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.ProcessedCount, Is.EqualTo(2));
            Assert.That(result.EventsCreated, Is.EqualTo(1));
            Assert.That(result.EventsUpdated, Is.EqualTo(1));
        }
        _geofenceEventWriterMock.Verify(w => w.CreateEntryEventAsync(It.IsAny<GeofenceEventDto>(), It.IsAny<CancellationToken>()), Times.Once);
        _geofenceEventWriterMock.Verify(w => w.UpdateExitEventAsync(createdEventId, t1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task ProcessPositionsAsync_ExitDebounce_DoesNotUpdate_WhenWithinInterval()
    {
        // Arrange
        var transporterId = Guid.NewGuid();
        var geofenceId = Guid.NewGuid();
        var t0 = DateTimeOffset.UtcNow;
        var t1 = t0.AddSeconds(10); // less than debounce interval (30s)

        var posEntry = new TransporterPositionDto(transporterId, 10.0, 20.0, t0);
        var posExit = new TransporterPositionDto(transporterId, 11.0, 21.0, t1);

        _geofenceEventReaderMock.Setup(r => r.GetOpenEventsForTransporterAsync(transporterId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        _geofenceReaderMock.SetupSequence(r => r.GetGeofenceIdsContainingPointAsync(It.IsAny<Guid>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([geofenceId])
            .ReturnsAsync([]);

        var createdEventId = Guid.NewGuid();
        var createdEvent = new GeofenceEventVm(createdEventId, transporterId, geofenceId, t0, null, posEntry.Latitude, posEntry.Longitude);
        _geofenceEventWriterMock.Setup(w => w.CreateEntryEventAsync(It.IsAny<GeofenceEventDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdEvent);

        var service = new GeofenceDetectionService(
            _geofenceReaderMock.Object,
            _geofenceEventReaderMock.Object,
            _geofenceEventWriterMock.Object);

        // Act
        var result = await service.ProcessPositionsAsync([posEntry, posExit], Guid.NewGuid(), CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.ProcessedCount, Is.EqualTo(2));
            Assert.That(result.EventsCreated, Is.EqualTo(1));
            Assert.That(result.EventsUpdated, Is.Zero);
        }
        _geofenceEventWriterMock.Verify(w => w.UpdateExitEventAsync(It.IsAny<Guid>(), It.IsAny<DateTimeOffset>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task ProcessPositionsAsync_MultipleGeofences_EntryCreatesMultipleEvents()
    {
        // Arrange
        var transporterId = Guid.NewGuid();
        var g1 = Guid.NewGuid();
        var g2 = Guid.NewGuid();
        var t0 = DateTimeOffset.UtcNow;

        var pos = new TransporterPositionDto(transporterId, 10.0, 20.0, t0);

        _geofenceEventReaderMock.Setup(r => r.GetOpenEventsForTransporterAsync(transporterId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        _geofenceReaderMock.Setup(r => r.GetGeofenceIdsContainingPointAsync(It.IsAny<Guid>(), pos.Latitude, pos.Longitude, It.IsAny<CancellationToken>()))
            .ReturnsAsync([g1, g2]);

        var created1 = new GeofenceEventVm(Guid.NewGuid(), transporterId, g1, t0, null, pos.Latitude, pos.Longitude);
        var created2 = new GeofenceEventVm(Guid.NewGuid(), transporterId, g2, t0, null, pos.Latitude, pos.Longitude);

        _geofenceEventWriterMock.SetupSequence(w => w.CreateEntryEventAsync(It.IsAny<GeofenceEventDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(created1)
            .ReturnsAsync(created2);

        var service = new GeofenceDetectionService(
            _geofenceReaderMock.Object,
            _geofenceEventReaderMock.Object,
            _geofenceEventWriterMock.Object);

        // Act
        var result = await service.ProcessPositionsAsync([pos], Guid.NewGuid(), CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.ProcessedCount, Is.EqualTo(1));
            Assert.That(result.EventsCreated, Is.EqualTo(2));
            Assert.That(result.EventsUpdated, Is.Zero);
        }
        _geofenceEventWriterMock.Verify(w => w.CreateEntryEventAsync(It.IsAny<GeofenceEventDto>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
    }
}
