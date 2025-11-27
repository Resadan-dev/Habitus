using Valoron.Activities.Domain;
using Valoron.BuildingBlocks;

namespace Valoron.Activities.Application;

public record CreateActivityCommand(
    string Title,
    string CategoryCode,
    int Difficulty,
    string MeasurementType, // "Binary" or "Quantifiable"
    MeasureUnit MeasurementUnit,
    decimal MeasurementTarget,
    Guid? ResourceId);
