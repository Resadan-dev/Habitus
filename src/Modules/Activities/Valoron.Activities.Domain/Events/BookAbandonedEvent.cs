using Valoron.BuildingBlocks;

namespace Valoron.Activities.Domain.Events;

public record BookAbandonedEvent(Guid BookId, Guid UserId);
