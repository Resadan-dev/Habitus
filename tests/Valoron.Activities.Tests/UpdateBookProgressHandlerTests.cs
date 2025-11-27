using Moq;
using Valoron.Activities.Application;
using Valoron.Activities.Domain;
using Valoron.Activities.Domain.Events;

namespace Valoron.Activities.Tests;

public class UpdateBookProgressHandlerTests
{
    private readonly Mock<IBookRepository> _bookRepositoryMock;
    private readonly UpdateBookProgressHandler _handler;

    public UpdateBookProgressHandlerTests()
    {
        _bookRepositoryMock = new Mock<IBookRepository>();
        _handler = new UpdateBookProgressHandler(_bookRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidEvent_UpdatesBook()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var book = new Book(bookId, "DDD", "Vernon", 300);
        var evt = new ActivityProgressLogged(Guid.NewGuid(), bookId, 20, "LRN", MeasureUnit.Pages);

        _bookRepositoryMock.Setup(r => r.GetByIdAsync(bookId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);

        // Act
        await _handler.Handle(evt, CancellationToken.None);

        // Assert
        Assert.Equal(20, book.CurrentPage);
        // _bookRepositoryMock.Verify(r => r.SaveAsync(book, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_BookNotFound_ThrowsException()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var evt = new ActivityProgressLogged(Guid.NewGuid(), bookId, 20, "LRN", MeasureUnit.Pages);

        _bookRepositoryMock.Setup(r => r.GetByIdAsync(bookId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Book?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(evt, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_NoResourceId_DoesNothing()
    {
        // Arrange
        var evt = new ActivityProgressLogged(Guid.NewGuid(), null, 20, "LRN", MeasureUnit.Pages);

        // Act
        await _handler.Handle(evt, CancellationToken.None);

        // Assert
        _bookRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        // _bookRepositoryMock.Verify(r => r.SaveAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
