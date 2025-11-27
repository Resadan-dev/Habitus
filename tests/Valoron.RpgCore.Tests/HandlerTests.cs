using Moq;
using Valoron.Activities.Domain;
using Valoron.Activities.Domain.Events;
using Valoron.RpgCore.Application.Handlers;
using Valoron.RpgCore.Domain;

namespace Valoron.RpgCore.Tests;

public class HandlerTests
{
    private readonly Mock<IPlayerRepository> _playerRepositoryMock;
    private readonly ActivityProgressLoggedHandler _progressHandler;
    private readonly BookFinishedHandler _bookFinishedHandler;

    public HandlerTests()
    {
        _playerRepositoryMock = new Mock<IPlayerRepository>();
        _progressHandler = new ActivityProgressLoggedHandler(_playerRepositoryMock.Object);
        _bookFinishedHandler = new BookFinishedHandler(_playerRepositoryMock.Object);
    }

    [Fact]
    public async Task ActivityProgressLogged_ShouldAddXp_WhenUnitIsPages()
    {
        // Arrange
        var player = new Player(Guid.NewGuid());
        _playerRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(player);

        var evt = new ActivityProgressLogged(Guid.NewGuid(), Guid.NewGuid(), 10, "LRN", MeasureUnit.Pages);

        // Act
        await _progressHandler.Handle(evt, CancellationToken.None);

        // Assert
        // 10 pages * 10 XP/page = 100 XP
        Assert.Equal(100, player.Xp);
        Assert.Equal(2, player.Level); // Should level up
    }

    [Fact]
    public async Task BookFinished_ShouldAdd500Xp()
    {
        // Arrange
        var player = new Player(Guid.NewGuid());
        _playerRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(player);

        var evt = new BookFinishedEvent(Guid.NewGuid());

        // Act
        await _bookFinishedHandler.Handle(evt, CancellationToken.None);

        // Assert
        Assert.Equal(500, player.Xp);
        Assert.Equal(3, player.Level); // 500 XP > 250 XP -> Level 3
    }
}
