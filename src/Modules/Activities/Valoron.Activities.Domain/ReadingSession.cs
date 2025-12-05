using Valoron.BuildingBlocks;

namespace Valoron.Activities.Domain;

public class ReadingSession : ValueObject
{
    public DateTime Date { get; }
    public TimeSpan Duration { get; }
    public int PagesRead { get; }

    private ReadingSession() { } // For EF Core

    public ReadingSession(DateTime date, TimeSpan duration, int pagesRead)
    {
        if (duration < TimeSpan.Zero) throw new ArgumentException("Duration cannot be negative", nameof(duration));
        if (pagesRead <= 0) throw new ArgumentException("Pages read must be greater than 0", nameof(pagesRead));

        Date = date;
        Duration = duration;
        PagesRead = pagesRead;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Date;
        yield return Duration;
        yield return PagesRead;
    }
}
