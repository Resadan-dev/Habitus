using Valoron.Activities.Domain;

namespace Valoron.Activities.Application;

public class LogReadingSessionHandler
{
    private readonly IActivityRepository _activityRepository;
    private readonly IBookRepository _bookRepository;

    public LogReadingSessionHandler(IActivityRepository activityRepository, IBookRepository bookRepository)
    {
        _activityRepository = activityRepository;
        _bookRepository = bookRepository;
    }

    public async Task Handle(LogReadingSessionCommand command, CancellationToken cancellationToken)
    {
        var activity = await _activityRepository.GetByIdAsync(command.ActivityId, cancellationToken);
        if (activity == null)
        {
            throw new InvalidOperationException($"Activity with ID {command.ActivityId} not found.");
        }

        if (activity.ResourceId == null)
        {
            throw new InvalidOperationException("Activity is not linked to a book.");
        }

        var book = await _bookRepository.GetByIdAsync(activity.ResourceId.Value, cancellationToken);
        if (book == null)
        {
            throw new InvalidOperationException($"Book with ID {activity.ResourceId} not found.");
        }

        book.AddPagesRead(command.PagesRead);
        activity.LogProgress(command.PagesRead);

        await _bookRepository.SaveAsync(book, cancellationToken);
        await _activityRepository.SaveAsync(activity, cancellationToken);
    }
}
