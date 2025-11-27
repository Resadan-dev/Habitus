
using Valoron.Activities.Domain;
using Valoron.BuildingBlocks;

namespace Valoron.Activities.Application;

public class CreateBookHandler
{
    private readonly IBookRepository _repository;
    private readonly ICurrentUserService _currentUserService;

    public CreateBookHandler(IBookRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<(Guid, IEnumerable<object>)> Handle(CreateBookCommand command, CancellationToken ct)
    {
        var book = new Book(Guid.NewGuid(), _currentUserService.UserId, command.Title, command.Author, command.TotalPages);
        
        await _repository.AddAsync(book, ct);
        
        return (book.Id, book.DomainEvents);
    }
}
