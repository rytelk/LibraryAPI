using System.Threading;
using System.Threading.Tasks;
using Library.Application.Commands.BookLoans.BookReturnCommand;
using Library.Domain.Services;
using MediatR;
using Moq;
using Xunit;

namespace Library.UnitTests.Application.Commands.BookLoans.BookReturnCommandTests
{
    public class BookReturnCommandHandlerTests
    {
        private readonly Mock<IBookReturnService> _bookReturnServiceMock;

        private BookReturnCommandHandler _sut =>
            new BookReturnCommandHandler(_bookReturnServiceMock.Object);

        public BookReturnCommandHandlerTests()
        {
            _bookReturnServiceMock = new Mock<IBookReturnService>();
        }


        [Fact]
        public async Task Handle_ValidCommand_BookReturned()
        {
            var bookId = 1;
            var userId = 1;
            var command = new BookReturnCommand()
            {
                BookId = bookId,
                UserId = userId
            };

            await _sut.Handle(command, CancellationToken.None);

            _bookReturnServiceMock.Verify(x => x.ReturnBook(userId, bookId), Times.Once);
        }
    }
}