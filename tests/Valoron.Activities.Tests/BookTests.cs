using Valoron.Activities.Domain;

namespace Valoron.Activities.Tests;

public class BookTests
{
    [Fact]
    public void Constructor_WithValidParameters_CreatesBook()
    {
        var id = Guid.NewGuid();
        var book = new Book(id, "DDD Distilled", "Vaughn Vernon", 250);

        Assert.Equal(id, book.Id);
        Assert.Equal("DDD Distilled", book.Title);
        Assert.Equal("Vaughn Vernon", book.Author);
        Assert.Equal(250, book.TotalPages);
        Assert.Equal(0, book.CurrentPage);
        Assert.Equal(BookStatus.ToRead, book.Status);
    }

    [Fact]
    public void StartReading_UpdatesStatus()
    {
        var book = new Book(Guid.NewGuid(), "Title", "Author", 100);
        book.StartReading();
        Assert.Equal(BookStatus.Reading, book.Status);
    }

    [Fact]
    public void AddPagesRead_UpdatesCurrentPage()
    {
        var book = new Book(Guid.NewGuid(), "Title", "Author", 100);
        book.StartReading();
        book.AddPagesRead(20);
        Assert.Equal(20, book.CurrentPage);
    }

    [Fact]
    public void AddPagesRead_AutoStartsReading()
    {
        var book = new Book(Guid.NewGuid(), "Title", "Author", 100);
        book.AddPagesRead(20);
        Assert.Equal(BookStatus.Reading, book.Status);
        Assert.Equal(20, book.CurrentPage);
    }

    [Fact]
    public void AddPagesRead_CompletesBook()
    {
        var book = new Book(Guid.NewGuid(), "Title", "Author", 100);
        book.AddPagesRead(100);
        Assert.Equal(BookStatus.Finished, book.Status);
        Assert.Equal(100, book.CurrentPage);
    }

    [Fact]
    public void AddPagesRead_CapsAtTotalPages()
    {
        var book = new Book(Guid.NewGuid(), "Title", "Author", 100);
        book.AddPagesRead(150);
        Assert.Equal(BookStatus.Finished, book.Status);
        Assert.Equal(100, book.CurrentPage);
    }
}
