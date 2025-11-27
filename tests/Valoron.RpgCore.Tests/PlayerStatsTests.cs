using Valoron.RpgCore.Domain;

namespace Valoron.RpgCore.Tests;

public class PlayerStatsTests
{
    [Fact]
    public void Default_ShouldReturnOnes()
    {
        var stats = PlayerStats.Default();
        Assert.Equal(1, stats.Strength);
        Assert.Equal(1, stats.Intellect);
        Assert.Equal(1, stats.Stamina);
    }

    [Fact]
    public void Increase_ShouldAddValues()
    {
        var stats = PlayerStats.Default();
        var newStats = stats.Increase(1, 2, 3);

        Assert.Equal(2, newStats.Strength);
        Assert.Equal(3, newStats.Intellect);
        Assert.Equal(4, newStats.Stamina);
    }

    [Fact]
    public void ValueObject_EqualityCheck()
    {
        var stats1 = PlayerStats.Default();
        var stats2 = PlayerStats.Default();

        Assert.Equal(stats1, stats2);
        Assert.NotSame(stats1, stats2);
    }
}
