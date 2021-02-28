using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Library.Application.Commands.Books.UpdateBookCommand;
using Library.Application.DTOs;
using Library.Domain.AggregatesModel.BookAggregate;
using Library.Domain.SeedWork;
using Library.Infrastructure.Services;
using Moq;
using Xunit;

namespace Library.UnitTests.Application.Commands.Books.UpdateBookCommandTests
{
    public class UpdateBookCommandHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IMapper<Book, BookDTO>> _bookMapperMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        private UpdateBookCommandHandler _sut =>
            new UpdateBookCommandHandler(_bookRepositoryMock.Object, _bookMapperMock.Object);

        public UpdateBookCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _bookMapperMock = new Mock<IMapper<Book, BookDTO>>();

            _bookRepositoryMock.SetupGet(x => x.UnitOfWork)
                .Returns(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_BookUpdated()
        {
            var bookId = 1;
            var newTitle = "NewTitle";
            var command = new UpdateBookCommand()
            {
                BookId = 1,
                Title = newTitle,
                Description = "Description",
                YearPublished = 2020,
                Author = new AuthorDTO()
                {
                    FirstName = "FirstName",
                    LastName = "LastName"
                }
            };
            var bookToUpdate = new Book("Title", new Author("FirstName", "LastName"), 2020, "Description");
            _bookRepositoryMock.Setup(x => x.GetAsync(bookId))
                .ReturnsAsync(bookToUpdate);

            await _sut.Handle(command, CancellationToken.None);

            _bookRepositoryMock.Verify(x => x.Update(bookToUpdate), Times.Once);
            bookToUpdate.Title.Should().Be(newTitle);
        }
    }
}