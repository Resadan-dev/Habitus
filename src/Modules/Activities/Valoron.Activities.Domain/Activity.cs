using Valoron.BuildingBlocks;

namespace Valoron.Activities.Domain;

public class Activity : Entity
{
    public string Title { get; private set; }
    public ActivityCategory Category { get; private set; }
    public Difficulty Difficulty { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    public bool IsCompleted => CompletedAt.HasValue;

    private Activity() { }

    public Activity(Guid id, string title, ActivityCategory category, Difficulty difficulty) : base(id)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title can't be empty");

        Title = title;
        Category = category;
        Difficulty = difficulty;
        CreatedAt = DateTime.UtcNow;
    }

    public void Complete()
    {
        if (IsCompleted)
            return;

        CompletedAt = DateTime.UtcNow;
        
        // TODO: AddDomainEvent(new ActivityCompletedEvent(this));
    }

    public void UpdateDifficulty(Difficulty newDifficulty)
    {
        if (IsCompleted)
            throw new InvalidOperationException("Impossible to change the difficulty of an ended activity");

        Difficulty = newDifficulty;
    }
}