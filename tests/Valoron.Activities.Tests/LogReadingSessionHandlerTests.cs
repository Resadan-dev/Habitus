using Moq;
using Valoron.Activities.Application;
using Valoron.Activities.Domain;
using Valoron.Activities.Domain.Events;

namespace Valoron.Activities.Tests;

public class LogReadingSessionHandlerTests
{
    private readonly Mock<IActivityRepository> _activityRepositoryMock;
    private readonly LogReadingSessionHandler _handler;

    public LogReadingSessionHandlerTests()
    {
        _activityRepositoryMock = new Mock<IActivityRepository>();
        _handler = new LogReadingSessionHandler(_activityRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_UpdatesActivityAndReturnsEvent()
    {
        // Arrange
        var activityId = Guid.NewGuid();
        var bookId = Guid.NewGuid();
        var pagesRead = 20;

        var measurement = ActivityMeasurement.CreateQuantifiable(MeasureUnit.Pages, 100);
        var activity = new Activity(activityId, "Read DDD", ActivityCategory.Learning, ActivityDifficulty.Medium, measurement, DateTime.UtcNow, bookId);

        _activityRepositoryMock.Setup(r => r.GetByIdAsync(activityId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(activity);

        var command = new LogReadingSessionCommand(activityId, pagesRead);

        // Act
        var events = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(20, activity.Measurement.CurrentValue);

        // _activityRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        
        Assert.Single(events);
        var evt = Assert.IsType<ActivityProgressLogged>(events.First());
        Assert.Equal(activityId, evt.ActivityId);
        Assert.Equal(bookId, evt.ResourceId);
        Assert.Equal(pagesRead, evt.Progress);
    }

    [Fact]
    public async Task Handle_ActivityNotFound_ThrowsException()
    {
        // Arrange
        var activityId = Guid.NewGuid();
        _activityRepositoryMock.Setup(r => r.GetByIdAsync(activityId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Activity?)null);

        var command = new LogReadingSessionCommand(activityId, 10);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ActivityNotLinkedToBook_ThrowsException()
    {
        // Arrange
        var activityId = Guid.NewGuid();
        
        var measurement = ActivityMeasurement.CreateQuantifiable(MeasureUnit.Pages, 100);
        var activity = new Activity(activityId, "Read DDD", ActivityCategory.Learning, ActivityDifficulty.Medium, measurement, DateTime.UtcNow, null); // No ResourceId

        _activityRepositoryMock.Setup(r => r.GetByIdAsync(activityId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(activity);

        var command = new LogReadingSessionCommand(activityId, 10);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
