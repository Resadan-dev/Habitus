using Valoron.BuildingBlocks;

namespace Valoron.RpgCore.Domain.Rules;

public static class XpCalculator
{
    public static int CalculateFromProgress(MeasureUnit unit, decimal progress)
    {
        return unit switch
        {
            MeasureUnit.Pages => (int)(progress * 10),
            _ => 0
        };
    }

    public const int BookFinishedBonus = 500;

    public static int CalculateRequiredXp(int currentLevel)
    {
        // Formula: 100 * (currentLevel ^ 1.5)
        return (int)(100 * Math.Pow(currentLevel, 1.5));
    }
}
