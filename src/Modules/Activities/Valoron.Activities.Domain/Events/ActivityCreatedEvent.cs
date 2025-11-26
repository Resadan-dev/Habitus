namespace Valoron.Activities.Domain.Events;

public record ActivityCreatedEvent(Guid ActivityId, string Title, string Category, int Difficulty, Guid? ResourceId);
