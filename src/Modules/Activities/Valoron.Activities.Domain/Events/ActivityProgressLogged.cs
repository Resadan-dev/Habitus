using Valoron.BuildingBlocks;

namespace Valoron.Activities.Domain.Events;

public record ActivityProgressLogged(Guid ActivityId, Guid UserId, Guid? ResourceId, decimal Progress, string CategoryCode, MeasureUnit Unit, TimeSpan? Duration);
