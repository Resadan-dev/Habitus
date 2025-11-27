namespace Valoron.Activities.Domain.Events;

public record ActivityCreatedEvent(Guid ActivityId, Guid UserId, string Title, string Category, int Difficulty, Guid? ResourceId);
