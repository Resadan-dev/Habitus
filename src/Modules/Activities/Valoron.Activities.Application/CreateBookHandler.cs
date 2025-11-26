
using Valoron.Activities.Domain;

namespace Valoron.Activities.Application;

public class CreateBookHandler
{
    private readonly IBookRepository _repository;

    public CreateBookHandler(IBookRepository repository)
    {
        _repository = repository;
    }

    public async Task<(Guid, IEnumerable<object>)> Handle(CreateBookCommand command, CancellationToken ct)
    {
        var book = new Book(Guid.NewGuid(), command.Title, command.Author, command.TotalPages);
        
        await _repository.AddAsync(book, ct);
        
        return (book.Id, book.DomainEvents);
    }
}
