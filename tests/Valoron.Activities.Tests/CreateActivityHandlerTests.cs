using Microsoft.Extensions.Time.Testing;
using Moq;
using Valoron.Activities.Application;
using Valoron.Activities.Domain;
using Valoron.BuildingBlocks;

namespace Valoron.Activities.Tests;

public class CreateActivityHandlerTests
{
    private readonly Mock<IActivityRepository> _activityRepositoryMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly FakeTimeProvider _fakeTimeProvider;
    private readonly CreateActivityHandler _handler;

    public CreateActivityHandlerTests()
    {
        _activityRepositoryMock = new Mock<IActivityRepository>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _fakeTimeProvider = new FakeTimeProvider();

        _currentUserServiceMock.Setup(x => x.UserId).Returns(Guid.NewGuid());

        _handler = new CreateActivityHandler(_activityRepositoryMock.Object, _fakeTimeProvider, _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesActivityWithCorrectTime()
    {
        // Arrange
        var fixedTime = new DateTimeOffset(2023, 10, 27, 12, 0, 0, TimeSpan.Zero);
        _fakeTimeProvider.SetUtcNow(fixedTime);

        var command = new CreateActivityCommand(
            "Test Activity",
            "ENV",
            1,
            "Binary",
            MeasureUnit.Count, // Default value, ignored for Binary
            0, // Default value, ignored for Binary
            null);

        Activity? capturedActivity = null;
        _activityRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Activity>(), It.IsAny<CancellationToken>()))
            .Callback<Activity, CancellationToken>((a, c) => capturedActivity = a)
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(capturedActivity);
        Assert.Equal(fixedTime.DateTime, capturedActivity.CreatedAt);
    }
}
