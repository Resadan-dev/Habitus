using Valoron.Activities.Application.Dtos;

namespace Valoron.Activities.Application.Queries;

public class BookQueryHandler
{
    private readonly IBookQueries _queries;

    public BookQueryHandler(IBookQueries queries)
    {
        _queries = queries;
    }

    public Task<IEnumerable<BookDto>> Handle(GetBooksQuery query, CancellationToken cancellationToken)
    {
        return _queries.GetBooksAsync(cancellationToken);
    }
}
