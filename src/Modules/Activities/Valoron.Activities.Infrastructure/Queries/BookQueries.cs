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
        var books = await _context.Books
            .Include(b => b.ReadingSessions)
            .ToListAsync(cancellationToken);

        var bookIds = books.Select(b => b.Id).ToList();
        var activities = await _context.Activities
            .Where(a => a.ResourceId != null && bookIds.Contains(a.ResourceId.Value))
            .ToDictionaryAsync(a => a.ResourceId.Value, a => a.Id, cancellationToken);

        return books.Select(b => new BookDto(
            b.Id,
            activities.ContainsKey(b.Id) ? activities[b.Id] : Guid.Empty,
            b.Title,
            b.Author,
            b.TotalPages,
            b.CurrentPage,
            b.Status.ToString(),
            b.AverageReadingSpeed,
            b.EstimateTimeRemaining()
        ));
    }

    public async Task<BookDto?> GetBookByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var book = await _context.Books
            .Include(b => b.ReadingSessions)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

        if (book == null) return null;

        var activityId = await _context.Activities
            .Where(a => a.ResourceId == book.Id)
            .Select(a => a.Id)
            .FirstOrDefaultAsync(cancellationToken);

        return new BookDto(
            book.Id,
            activityId,
            book.Title,
            book.Author,
            book.TotalPages,
            book.CurrentPage,
            book.Status.ToString(),
            book.AverageReadingSpeed,
            book.EstimateTimeRemaining()
        );
    }
}
