using Valoron.BuildingBlocks;
using Valoron.Activities.Domain.Events;

namespace Valoron.Activities.Domain;

public class Activity : Entity
{
    public string Title { get; private set; }
    public ActivityCategory Category { get; private set; }
    public ActivityDifficulty Difficulty { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public Guid? ResourceId { get; private set; }

    public ActivityMeasurement Measurement { get; private set; }

    public bool IsCompleted => Measurement.IsMet();

    private Activity() { }

    public Activity(Guid id, string title, ActivityCategory category, ActivityDifficulty difficulty, ActivityMeasurement measurement, Guid? resourceId = null) 
        : base(id)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title can't be empty");

        Title = title;
        Category = category;
        Difficulty = difficulty;
        Measurement = measurement;
        ResourceId = resourceId;
        CreatedAt = DateTime.UtcNow;

        AddDomainEvent(new ActivityCreatedEvent(Id, Title, Category.Code, Difficulty.Value, ResourceId));
    }

    public void LogProgress(decimal value)
    {
        if (IsCompleted && Measurement.Unit == MeasureUnit.None)
            return;

        Measurement = Measurement.WithProgress(Measurement.CurrentValue + value);

        AddDomainEvent(new ActivityProgressLogged(Id, ResourceId, value));

        if (IsCompleted && CompletedAt == null)
        {
            CompletedAt = DateTime.UtcNow;
            AddDomainEvent(new ActivityCompletedEvent(Id, ResourceId));
        }
        else if (!IsCompleted && CompletedAt != null)
        {
            CompletedAt = null; // Regression handling
        }
    }

    public void UpdateDifficulty(ActivityDifficulty newDifficulty)
    {
        if (IsCompleted)
            throw new InvalidOperationException("Impossible to change the difficulty of an ended activity");

        Difficulty = newDifficulty;
    }
}
