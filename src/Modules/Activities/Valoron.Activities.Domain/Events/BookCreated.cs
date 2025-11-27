namespace Valoron.Activities.Domain.Events;

public record BookCreated(Guid Id, Guid UserId, string Title, string Author, int TotalPages);
