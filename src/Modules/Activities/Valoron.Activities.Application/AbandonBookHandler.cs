using Valoron.Activities.Domain;
using Valoron.BuildingBlocks;

namespace Valoron.Activities.Application;

public class AbandonBookHandler
{
    private readonly IBookRepository _repository;
    private readonly ICurrentUserService _currentUserService;

    public AbandonBookHandler(IBookRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<object>> Handle(AbandonBookCommand command, CancellationToken cancellationToken)
    {
        var book = await _repository.GetByIdAsync(command.BookId, cancellationToken);
        if (book == null)
        {
            throw new InvalidOperationException($"Book with ID {command.BookId} not found.");
        }

        if (book.UserId != _currentUserService.UserId)
        {
             throw new UnauthorizedAccessException("You can only abandon your own books.");
        }

        book.Abandon();
        await _repository.SaveChangesAsync(cancellationToken);

        // Wolverine will automatically persist changes due to transactional middleware
        // We return events to be published
        return book.DomainEvents;
    }
}
