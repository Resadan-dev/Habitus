using Valoron.BuildingBlocks;

namespace Valoron.RpgCore.Domain;

public class PlayerStats : ValueObject
{
    public int Strength { get; }
    public int Intellect { get; }
    public int Stamina { get; }

    private PlayerStats(int strength, int intellect, int stamina)
    {
        Strength = strength;
        Intellect = intellect;
        Stamina = stamina;
    }

    public static PlayerStats Default() => new(1, 1, 1);

    public PlayerStats Increase(int strength, int intellect, int stamina)
    {
        return new PlayerStats(Strength + strength, Intellect + intellect, Stamina + stamina);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Strength;
        yield return Intellect;
        yield return Stamina;
    }
}
