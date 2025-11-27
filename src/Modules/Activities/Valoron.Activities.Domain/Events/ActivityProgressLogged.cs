namespace Valoron.Activities.Domain.Events;

public record ActivityProgressLogged(Guid ActivityId, Guid? ResourceId, decimal Progress, string CategoryCode, MeasureUnit Unit);
