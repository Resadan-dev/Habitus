using Valoron.BuildingBlocks;

namespace Valoron.Activities.Domain;

public class ActivityMeasurement : ValueObject
{
    public MeasureUnit Unit { get; }
    public decimal TargetValue { get; }
    public decimal CurrentValue { get; }

    private ActivityMeasurement(MeasureUnit unit, decimal targetValue, decimal currentValue)
    {
        if (targetValue <= 0) throw new ArgumentException("L'objectif doit être supérieur à 0");
        if (currentValue < 0) throw new ArgumentException("La valeur actuelle ne peut pas être négative");

        Unit = unit;
        TargetValue = targetValue;
        CurrentValue = currentValue;
    }

    // Factory pour une tâche binaire (Fait/Pas fait)
    public static ActivityMeasurement CreateBinary() 
        => new(MeasureUnit.None, 1, 0);

    // Factory pour une tâche quantitative (ex: 30 minutes)
    public static ActivityMeasurement CreateQuantifiable(MeasureUnit unit, decimal target) 
        => new(unit, target, 0);

    // Méthode pour créer un NOUVEL objet avec la progression mise à jour (Immuabilité !)
    public ActivityMeasurement WithProgress(decimal newValue)
    {
        // On peut imaginer bloquer si ça dépasse, ou permettre le "Over-achievement" (Bonus XP)
        return new ActivityMeasurement(Unit, TargetValue, newValue);
    }

    // Est-ce que l'activité est considérée comme finie ?
    public bool IsMet() => CurrentValue >= TargetValue;

    // Pourcentage de complétion (utile pour l'UI ou l'XP partielle)
    public decimal CompletionPercentage() 
    {
        if (TargetValue == 0) return 0;
        return Math.Min(CurrentValue / TargetValue, 1.0m); // Max 100% pour l'affichage standard
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Unit;
        yield return TargetValue;
        yield return CurrentValue;
    }
}
