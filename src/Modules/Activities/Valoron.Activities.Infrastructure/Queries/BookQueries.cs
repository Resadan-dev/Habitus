using Microsoft.EntityFrameworkCore;
using Valoron.Activities.Application.Dtos;
using Valoron.Activities.Application.Queries;
using Valoron.Activities.Infrastructure.Persistence;

namespace Valoron.Activities.Infrastructure.Queries;

public class BookQueries : IBookQueries
{
    private readonly ActivitiesDbContext _context;

    public BookQueries(ActivitiesDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BookDto>> GetBooksAsync(CancellationToken cancellationToken)
    {
        return await _context.Books
            .Select(b => new BookDto(
                b.Id,
                b.Title,
                b.Author,
                b.TotalPages,
                b.CurrentPage,
                b.Status.ToString()
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<BookDto?> GetBookByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Books
            .Where(b => b.Id == id)
            .Select(b => new BookDto(
                b.Id,
                b.Title,
                b.Author,
                b.TotalPages,
                b.CurrentPage,
                b.Status.ToString()
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
