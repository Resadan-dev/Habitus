using Valoron.BuildingBlocks;

namespace Valoron.Activities.Domain;

public class Difficulty : ValueObject
{
    public int Value { get; } // 1 to 10

    private Difficulty(int value)
    {
        if (value < 1 || value > 10)
            throw new ArgumentException("Difficulty must be between 1 and 10");

        Value = value;
    }

    public static Difficulty Create(int value)
    {
        return new Difficulty(value);
    }

    // some presets, to begin
    public static Difficulty Easy => new(1);
    public static Difficulty Medium => new(5);
    public static Difficulty Hard => new(8);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}