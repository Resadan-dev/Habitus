using Valoron.Activities.Application.Dtos;

namespace Valoron.Activities.Application.Queries;

public interface IBookQueries
{
    Task<IEnumerable<BookDto>> GetBooksAsync(CancellationToken cancellationToken);
    Task<BookDto?> GetBookByIdAsync(Guid id, CancellationToken cancellationToken);
}
