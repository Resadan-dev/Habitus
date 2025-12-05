using Valoron.Activities.Domain;

namespace Valoron.Activities.Application.Dtos;

public record ActivityDto(
    Guid Id,
    string Title,
    ActivityCategory Category,
    ActivityDifficulty Difficulty,
    bool IsCompleted,
    DateTime CreatedAt,
    DateTime? CompletedAt,
    Guid? ResourceId,
    ActivityMeasurement Measurement,
    string? Status = null);
