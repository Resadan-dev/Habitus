using Valoron.BuildingBlocks;

namespace Valoron.Activities.Domain.Events;

public record ActivityUncompletedEvent(Guid ActivityId, Guid? ResourceId);
