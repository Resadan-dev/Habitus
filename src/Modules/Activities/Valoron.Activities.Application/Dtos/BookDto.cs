namespace Valoron.Activities.Application.Dtos;

public record BookDto(
    Guid Id,
    Guid ActivityId,
    string Title,
    string Author,
    int TotalPages,
    int CurrentPage,
    string Status,
    double? AverageReadingSpeed,
    TimeSpan? EstimatedTimeRemaining
);
