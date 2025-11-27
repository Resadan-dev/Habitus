using Valoron.RpgCore.Domain;
using Valoron.RpgCore.Domain.Events;

namespace Valoron.RpgCore.Tests;

public class PlayerTests
{
    [Fact]
    public void NewPlayer_ShouldHaveZeroXpAndLevelOne()
    {
        var player = new Player(Guid.NewGuid());
        Assert.Equal(0, player.Xp);
        Assert.Equal(1, player.Level);
    }

    [Fact]
    public void AddXp_ShouldIncreaseXp()
    {
        var player = new Player(Guid.NewGuid());
        player.AddXp(50);
        Assert.Equal(50, player.Xp);
    }

    [Fact]
    public void AddXp_ShouldLevelUp_WhenThresholdReached()
    {
        var player = new Player(Guid.NewGuid());
        
        // Level 1 -> 2 needs 100 XP
        player.AddXp(100);
        
        Assert.Equal(2, player.Level);
        Assert.Contains(player.DomainEvents, e => e is PlayerLeveledUpEvent ple && ple.NewLevel == 2);
    }

    [Fact]
    public void AddXp_ShouldLevelUpMultipleTimes()
    {
        var player = new Player(Guid.NewGuid());
        
        // Level 1 -> 3 needs 250 XP total
        player.AddXp(250);
        
        Assert.Equal(3, player.Level);
        Assert.Contains(player.DomainEvents, e => e is PlayerLeveledUpEvent ple && ple.NewLevel == 2);
        Assert.Contains(player.DomainEvents, e => e is PlayerLeveledUpEvent ple && ple.NewLevel == 3);
    }
}
