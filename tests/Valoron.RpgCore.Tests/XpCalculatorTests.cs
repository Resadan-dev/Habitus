using Valoron.RpgCore.Domain.Rules;

namespace Valoron.RpgCore.Tests;

public class XpCalculatorTests
{
    [Theory]
    [InlineData(1, 100)]
    [InlineData(2, 282)] // 100 * 2^1.5 = 100 * 2.8284 = 282.84 -> 282
    [InlineData(3, 519)] // 100 * 3^1.5 = 100 * 5.1961 = 519.61 -> 519
    [InlineData(4, 800)] // 100 * 4^1.5 = 100 * 8 = 800
    [InlineData(5, 1118)] // 100 * 5^1.5 = 100 * 11.1803 = 1118.03 -> 1118
    public void CalculateRequiredXp_ReturnsCorrectValue(int level, int expectedXp)
    {
        // Act
        int result = XpCalculator.CalculateRequiredXp(level);

        // Assert
        Assert.Equal(expectedXp, result);
    }
}
