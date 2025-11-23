namespace Valoron.Activities.Application;

public record LogReadingSessionCommand(Guid ActivityId, int PagesRead);
