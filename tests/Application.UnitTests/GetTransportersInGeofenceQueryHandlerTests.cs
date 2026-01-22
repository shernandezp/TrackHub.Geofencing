using TrackHub.Manager.Application.TransportersInGeofence.Queries.Get;
using TrackHub.Manager.Domain.Interfaces;
using Common.Application.Interfaces;

namespace TrackHub.Manager.Application.UnitTests.TransportersInGeofence.Queries.Get;

[TestFixture]
public class GetTransportersInGeofenceQueryHandlerTests
{
    private Mock<ITransportersInGeofence> _readerMock = null!;
    private Mock<IUserReader> _userReaderMock = null!;
    private Mock<IUser> _userMock = null!;

    [SetUp]
    public void SetUp()
    {
        _readerMock = new Mock<ITransportersInGeofence>();
        _userReaderMock = new Mock<IUserReader>();
        _userMock = new Mock<IUser>();
        _userMock.Setup(u => u.Id).Returns(Guid.NewGuid().ToString());
    }

    [Test]
    public async Task Handle_ShouldReturnTransportersForUserAccount()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var items = new[] { new TransporterInGeofenceVm(userId, "name", Guid.NewGuid(), "gname") };
        _userReaderMock.Setup(r => r.GetUserAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new UserVm { UserId = userId, Username = "u", AccountId = accountId });
        _readerMock.Setup(r => r.GetTransportersInGeofencesAsync(accountId, It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(items);

        var handler = new GetTransportersInGeofenceQueryHandler(_readerMock.Object, _userReaderMock.Object, _userMock.Object);
        var query = new GetTransportersInGeofenceQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.EqualTo(items));
        _userReaderMock.Verify(r => r.GetUserAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        _readerMock.Verify(r => r.GetTransportersInGeofencesAsync(accountId, It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void Constructor_ShouldThrowUnauthorizedAccessException_WhenUserIdIsNull()
    {
        // Arrange
        _userMock.Setup(u => u.Id).Returns(() => null);

        // Act & Assert
        Assert.Throws<UnauthorizedAccessException>(() => new GetTransportersInGeofenceQueryHandler(_readerMock.Object, _userReaderMock.Object, _userMock.Object));
    }
}
