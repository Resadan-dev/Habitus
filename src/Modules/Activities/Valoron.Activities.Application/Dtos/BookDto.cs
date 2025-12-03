namespace Valoron.Activities.Application.Dtos;

public record BookDto(
    Guid Id,
    string Title,
    string Author,
    int TotalPages,
    int CurrentPage,
    string Status
);
