using Valoron.BuildingBlocks;

namespace Valoron.Activities.Domain;

public class ActivityMeasurement : ValueObject
{
    public MeasureUnit Unit { get; }
    public decimal TargetValue { get; }
    public decimal CurrentValue { get; }

    private ActivityMeasurement(MeasureUnit unit, decimal targetValue, decimal currentValue)
    {
        if (targetValue <= 0) throw new ArgumentException("Target value must be greater than 0");
        if (currentValue < 0) throw new ArgumentException("Current value cannot be negative");

        Unit = unit;
        TargetValue = targetValue;
        CurrentValue = currentValue;
    }

    // Factory for binary tasks (Done / Not done)
    public static ActivityMeasurement CreateBinary() 
        => new(MeasureUnit.None, 1, 0);

    // Factory for quantifiable tasks (e.g. 30 minutes)
    public static ActivityMeasurement CreateQuantifiable(MeasureUnit unit, decimal target) 
        => new(unit, target, 0);

    // Creates a NEW object with updated progress (Immutability)
    public ActivityMeasurement WithProgress(decimal newValue)
    {
        // Cap at TargetValue if it's not None unit (which has target 1 but might behave differently, though here we treat it as binary)
        // Actually, for quantifiable, we want to cap.
        // For binary, target is 1, so capping at 1 is correct.
        
        decimal cappedValue = newValue;
        if (TargetValue > 0 && newValue > TargetValue)
        {
            cappedValue = TargetValue;
        }

        return new ActivityMeasurement(Unit, TargetValue, cappedValue);
    }

    public bool IsMet() => CurrentValue >= TargetValue;

    // Completion percentage (useful for UI or partial XP)
    public decimal CompletionPercentage() 
    {
        if (TargetValue == 0) return 0;
        return Math.Min(CurrentValue / TargetValue, 1.0m); // Max 100% for standard display
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Unit;
        yield return TargetValue;
        yield return CurrentValue;
    }
}
