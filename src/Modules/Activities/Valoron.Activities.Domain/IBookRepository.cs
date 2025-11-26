namespace Valoron.Activities.Domain;

public interface IBookRepository
{
    Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Book book, CancellationToken cancellationToken = default);

}
