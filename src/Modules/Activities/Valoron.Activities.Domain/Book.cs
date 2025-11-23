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

    private Book() { }

    public Book(Guid id, string title, string author, int totalPages) : base(id)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title cannot be empty");
        if (string.IsNullOrWhiteSpace(author)) throw new ArgumentException("Author cannot be empty");
        if (totalPages <= 0) throw new ArgumentException("Total pages must be greater than 0");

        Title = title;
        Author = author;
        TotalPages = totalPages;
        CurrentPage = 0;
        Status = BookStatus.ToRead;
    }

    public void StartReading()
    {
        if (Status == BookStatus.Finished) throw new InvalidOperationException("Book is already finished");
        Status = BookStatus.Reading;
    }

    public void AddPagesRead(int pages)
    {
        if (pages <= 0) throw new ArgumentException("Pages read must be greater than 0");
        if (Status == BookStatus.Finished) throw new InvalidOperationException("Book is already finished");

        if (Status == BookStatus.ToRead)
        {
            StartReading();
        }

        CurrentPage += pages;

        if (CurrentPage >= TotalPages)
        {
            Finish();
        }
    }

    public void Finish()
    {
        CurrentPage = TotalPages;
        Status = BookStatus.Finished;
    }
}
