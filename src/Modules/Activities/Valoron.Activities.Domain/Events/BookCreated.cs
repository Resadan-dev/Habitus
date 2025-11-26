namespace Valoron.Activities.Domain.Events;

public record BookCreated(Guid Id, string Title, string Author, int TotalPages);
