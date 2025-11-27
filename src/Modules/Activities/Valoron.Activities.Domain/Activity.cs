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
    public Guid UserId { get; private set; }

    public ActivityMeasurement Measurement { get; private set; }

    public bool IsCompleted => Measurement.IsMet();

    private Activity() { }

    public Activity(Guid id, Guid userId, string title, ActivityCategory category, ActivityDifficulty difficulty, ActivityMeasurement measurement, DateTime createdAt, Guid? resourceId = null) 
        : base(id)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title can't be empty");

        Title = title;
        Category = category;
        Difficulty = difficulty;
        Measurement = measurement;
        ResourceId = resourceId;
        CreatedAt = createdAt;
        UserId = userId;

        AddDomainEvent(new ActivityCreatedEvent(Id, UserId, Title, Category.Code, Difficulty.Value, ResourceId));
    }

    public void LogProgress(decimal value, DateTime now)
    {
        if (IsCompleted && Measurement.Unit == MeasureUnit.None)
            return;

        bool wasCompleted = IsCompleted;

        Measurement = Measurement.WithProgress(Measurement.CurrentValue + value);

        AddDomainEvent(new ActivityProgressLogged(Id, UserId, ResourceId, value, Category.Code, Measurement.Unit));

        if (IsCompleted && !wasCompleted)
        {
            CompletedAt = now;
            AddDomainEvent(new ActivityCompletedEvent(Id, ResourceId));
        }
        else if (!IsCompleted && wasCompleted)
        {
            CompletedAt = null; // Regression handling
            AddDomainEvent(new ActivityUncompletedEvent(Id, ResourceId));
        }
    }

    public void UpdateDifficulty(ActivityDifficulty newDifficulty)
    {
        if (IsCompleted)
            throw new InvalidOperationException("Impossible to change the difficulty of an ended activity");

        Difficulty = newDifficulty;
    }
}
