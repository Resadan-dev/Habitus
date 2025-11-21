using Valoron.BuildingBlocks;

namespace Valoron.Activities.Domain;

public class ActivityDifficulty : ValueObject
{
    public int Value { get; } // 1 to 10

    private ActivityDifficulty(int value)
    {
        if (value < 1 || value > 10)
            throw new ArgumentException("Difficulty must be between 1 and 10");

        Value = value;
    }

    public static ActivityDifficulty Create(int value)
    {
        return new ActivityDifficulty(value);
    }

    // some presets, to begin
    public static ActivityDifficulty Easy => new(1);
    public static ActivityDifficulty Medium => new(5);
    public static ActivityDifficulty Hard => new(8);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}