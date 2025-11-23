using Moq;
using Valoron.Activities.Application;
using Valoron.Activities.Domain;

namespace Valoron.Activities.Tests;

public class LogReadingSessionHandlerTests
{
    private readonly Mock<IActivityRepository> _activityRepositoryMock;
    private readonly Mock<IBookRepository> _bookRepositoryMock;
    private readonly LogReadingSessionHandler _handler;

    public LogReadingSessionHandlerTests()
    {
        _activityRepositoryMock = new Mock<IActivityRepository>();
        _bookRepositoryMock = new Mock<IBookRepository>();
        _handler = new LogReadingSessionHandler(_activityRepositoryMock.Object, _bookRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_UpdatesBookAndActivity()
    {
        // Arrange
        var activityId = Guid.NewGuid();
        var bookId = Guid.NewGuid();
        var pagesRead = 20;

        var measurement = ActivityMeasurement.CreateQuantifiable(MeasureUnit.Pages, 100);
        var activity = new Activity(activityId, "Read DDD", ActivityCategory.Learning, ActivityDifficulty.Medium, measurement, bookId);
        var book = new Book(bookId, "DDD", "Vernon", 300);

        _activityRepositoryMock.Setup(r => r.GetByIdAsync(activityId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(activity);
        _bookRepositoryMock.Setup(r => r.GetByIdAsync(bookId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);

        var command = new LogReadingSessionCommand(activityId, pagesRead);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(20, book.CurrentPage);
        Assert.Equal(20, activity.Measurement.CurrentValue);

        _bookRepositoryMock.Verify(r => r.SaveAsync(book, It.IsAny<CancellationToken>()), Times.Once);
        _activityRepositoryMock.Verify(r => r.SaveAsync(activity, It.IsAny<CancellationToken>()), Times.Once);
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
    public async Task Handle_BookNotFound_ThrowsException()
    {
        // Arrange
        var activityId = Guid.NewGuid();
        var bookId = Guid.NewGuid();
        
        var measurement = ActivityMeasurement.CreateQuantifiable(MeasureUnit.Pages, 100);
        var activity = new Activity(activityId, "Read DDD", ActivityCategory.Learning, ActivityDifficulty.Medium, measurement, bookId);

        _activityRepositoryMock.Setup(r => r.GetByIdAsync(activityId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(activity);
        _bookRepositoryMock.Setup(r => r.GetByIdAsync(bookId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Book?)null);

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
        var activity = new Activity(activityId, "Read DDD", ActivityCategory.Learning, ActivityDifficulty.Medium, measurement, null); // No ResourceId

        _activityRepositoryMock.Setup(r => r.GetByIdAsync(activityId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(activity);

        var command = new LogReadingSessionCommand(activityId, 10);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
