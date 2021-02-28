using System.Threading;
using System.Threading.Tasks;
using Library.Application.Commands.Books.DeleteBookCommand;
using Library.Domain.AggregatesModel.BookAggregate;
using Library.Domain.SeedWork;
using Moq;
using Xunit;

namespace Library.UnitTests.Application.Commands.Books.DeleteBookCommandTests
{
    public class DeleteBookCommandHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        private DeleteBookCommandHandler _sut =>
            new DeleteBookCommandHandler(_bookRepositoryMock.Object);

        public DeleteBookCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _bookRepositoryMock = new Mock<IBookRepository>();

            _bookRepositoryMock.SetupGet(x => x.UnitOfWork)
                .Returns(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_BookDeleted()
        {
            var bookId = 1;
            var command = new DeleteBookCommand()
            {
                BookId = bookId
            };
            _bookRepositoryMock.Setup(x => x.GetAsync(bookId))
                .ReturnsAsync(new Book("Title", new Author("FirstName", "LastName"), 2020, "Description"));

            await _sut.Handle(command, CancellationToken.None);

            _bookRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Once);
        }
    }
}