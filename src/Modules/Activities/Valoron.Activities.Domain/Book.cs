using Valoron.BuildingBlocks;

namespace Valoron.Activities.Domain;

public enum BookStatus
{
    ToRead,
    Reading,
    Finished,
    Abandoned
}

public class Book : Entity
{
    public string Title { get; private set; }
    public string Author { get; private set; }
    public int TotalPages { get; private set; }
    public int CurrentPage { get; private set; }
    public BookStatus Status { get; private set; }
    public Guid UserId { get; private set; }

    private readonly List<ReadingSession> _readingSessions = new();
    public IReadOnlyCollection<ReadingSession> ReadingSessions => _readingSessions.AsReadOnly();

    public double? AverageReadingSpeed
    {
        get
        {
            if (!_readingSessions.Any()) return null;
            var totalPages = _readingSessions.Sum(s => s.PagesRead);
            var totalHours = _readingSessions.Sum(s => s.Duration.TotalHours);
            
            if (totalHours == 0) return null; // Avoid division by zero
            return totalPages / totalHours;
        }
    }

    public TimeSpan? EstimateTimeRemaining()
    {
        if (Status == BookStatus.Finished) return TimeSpan.Zero;
        
        var speed = AverageReadingSpeed;
        if (speed == null || speed <= 0) return null;

        var pagesRemaining = TotalPages - CurrentPage;
        if (pagesRemaining <= 0) return TimeSpan.Zero;

        var hoursRemaining = pagesRemaining / speed.Value;
        return TimeSpan.FromHours(hoursRemaining);
    }

    private Book() { }

    public Book(Guid id, Guid userId, string title, string author, int totalPages) : base(id)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title cannot be empty");
        if (string.IsNullOrWhiteSpace(author)) throw new ArgumentException("Author cannot be empty");
        if (totalPages <= 0) throw new ArgumentException("Total pages must be greater than 0");

        Title = title;
        Author = author;
        TotalPages = totalPages;
        CurrentPage = 0;
        Status = BookStatus.ToRead;
        UserId = userId;

        AddDomainEvent(new Events.BookCreated(Id, UserId, Title, Author, TotalPages));
    }

    public void StartReading()
    {
        if (Status == BookStatus.Finished) throw new InvalidOperationException("Book is already finished");
        Status = BookStatus.Reading;
        AddDomainEvent(new Events.BookStartedEvent(Id, UserId));
    }

    public void AddPagesRead(int pages, TimeSpan? duration = null, DateTime? date = null)
    {
        if (pages <= 0) throw new ArgumentException("Pages read must be greater than 0");
        if (Status == BookStatus.Finished) throw new InvalidOperationException("Book is already finished");

        if (Status == BookStatus.ToRead)
        {
            StartReading();
        }

        CurrentPage += pages;
        
        // Log the session if duration is provided
        if (duration.HasValue)
        {
            // We use the passed date or default to now (assuming the caller handles time provider, 
            // but domain entities usually receive dates, they don't ask for them unless injected).
            // Here 'date' is optional. If not provided, we might not be able to store 'When' properly 
            // without a TimeProvider. 
            // Ideally, the command handler passes the current time from TimeProvider.
            
            var sessionDate = date ?? DateTime.MinValue; // Should be passed by handler
            _readingSessions.Add(new ReadingSession(sessionDate, duration.Value, pages));
        }

        if (CurrentPage >= TotalPages)
        {
            Finish();
        }
    }

    public void Finish()
    {
        CurrentPage = TotalPages;
        Status = BookStatus.Finished;
        AddDomainEvent(new Events.BookFinishedEvent(Id, UserId));
    }

    public void Abandon()
    {
        if (Status == BookStatus.Finished) throw new InvalidOperationException("Cannot abandon a finished book");
        
        Status = BookStatus.Abandoned;
        AddDomainEvent(new Events.BookAbandonedEvent(Id, UserId));
    }
}
