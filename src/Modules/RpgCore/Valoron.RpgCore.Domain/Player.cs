using Valoron.BuildingBlocks;
using Valoron.RpgCore.Domain.Events;

namespace Valoron.RpgCore.Domain;

public class Player : Entity
{
    public int Xp { get; private set; }
    public int Level { get; private set; }
    public PlayerStats Stats { get; private set; }

    private Player() { }

    public Player(Guid userId) : base(userId)
    {
        Xp = 0;
        Level = 1;
        Stats = PlayerStats.Default();
    }

    public void AddXp(int amount)
    {
        if (amount < 0) throw new ArgumentException("XP amount cannot be negative");

        Xp += amount;
        CheckLevelUp();
    }

    public void GainXpFromActivity(MeasureUnit unit, decimal progress)
    {
        int xp = Rules.XpCalculator.CalculateFromProgress(unit, progress);
        if (xp > 0)
        {
            AddXp(xp);
        }
    }

    private void CheckLevelUp()
    {
        while (ShouldLevelUp(Level, Xp))
        {
            Level++;
            Stats = Stats.Increase(1, 1, 1); // Simple +1 to all stats per level for now
            AddDomainEvent(new PlayerLeveledUpEvent(Id, Level));
        }
    }

    private bool ShouldLevelUp(int currentLevel, int currentXp)
    {
        int xpRequiredForNextLevel = Rules.XpCalculator.CalculateRequiredXp(currentLevel);
        return currentXp >= xpRequiredForNextLevel;
    }
}
