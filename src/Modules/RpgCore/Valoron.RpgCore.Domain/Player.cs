using Valoron.BuildingBlocks;
using Valoron.RpgCore.Domain.Events;

namespace Valoron.RpgCore.Domain;

public class Player : Entity
{
    public int Xp { get; private set; }
    public int Level { get; private set; }

    private Player() { }

    public Player(Guid id) : base(id)
    {
        Xp = 0;
        Level = 1;
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
        int originalLevel = Level;

        // Level 1 -> 2: 100 XP
        // Level 2 -> 3: 250 XP
        // Simple logic for now as per requirements, can be expanded to a table or formula later.
        
        // Use the while loop to handle sequential level ups and event generation correctly.


        // Handle multiple level ups if necessary, though with current thresholds it's sequential.
        // If we get 500 XP at once from level 1:
        // 0 -> 500. 
        // Lvl 1 -> Lvl 3 directly? Or Lvl 1 -> Lvl 2 -> Lvl 3?
        // Let's make it robust.
        
        // Re-evaluating based on "250xp pour passer niveau 3". 
        // Does it mean 250 TOTAL or 250 MORE? 
        // Usually RPGs are cumulative. 
        // Lvl 1: 0-99
        // Lvl 2: 100-249
        // Lvl 3: 250+
        
        // Let's implement a loop or just checks.
        
        while (ShouldLevelUp(Level, Xp))
        {
            Level++;
            AddDomainEvent(new PlayerLeveledUpEvent(Id, Level));
        }
    }

    private bool ShouldLevelUp(int currentLevel, int currentXp)
    {
        return currentLevel switch
        {
            1 => currentXp >= 100,
            2 => currentXp >= 250,
            _ => false // Cap at level 3 for now as per requirements
        };
    }
}
