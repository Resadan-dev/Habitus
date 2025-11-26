using Valoron.Activities.Domain;
using Valoron.Activities.Domain.Events;

namespace Valoron.Activities.Application;

public class UpdateBookProgressHandler
{
    private readonly IBookRepository _bookRepository;

    public UpdateBookProgressHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task Handle(ActivityProgressLogged @event, CancellationToken cancellationToken)
    {
        if (@event.ResourceId == null)
            return;

        var book = await _bookRepository.GetByIdAsync(@event.ResourceId.Value, cancellationToken);
        if (book == null)
        {
            throw new InvalidOperationException($"Book with ID {@event.ResourceId} not found.");
        }

        book.AddPagesRead((int)@event.Progress);
        await _bookRepository.SaveChangesAsync(cancellationToken);
    }
}
