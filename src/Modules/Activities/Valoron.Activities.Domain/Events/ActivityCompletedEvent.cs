namespace Valoron.Activities.Domain.Events;

public record ActivityCompletedEvent(Guid ActivityId, Guid? ResourceId);
