using System.Threading;
using System.Threading.Tasks;
using Library.Application.Commands.Books.CreateBookCommand;
using Library.Application.DTOs;
using Library.Domain.AggregatesModel.BookAggregate;
using Library.Domain.SeedWork;
using Library.Infrastructure.Services;
using Moq;
using Xunit;

namespace Library.UnitTests.Application.Commands.Books.CreateBookCommandTests
{
    public class CreateBookCommandHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IMapper<Book, BookDTO>> _bookMapperMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        private CreateBookCommandHandler _sut =>
            new CreateBookCommandHandler(_bookRepositoryMock.Object, _bookMapperMock.Object);

        public CreateBookCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _bookMapperMock = new Mock<IMapper<Book, BookDTO>>();

            _bookRepositoryMock.SetupGet(x => x.UnitOfWork)
                .Returns(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_BookCreated()
        {
            var command = new CreateBookCommand()
            {
                Title = "Title",
                Description = "Description",
                YearPublished = 2020,
                Author = new AuthorDTO()
                {
                    FirstName = "FirstName",
                    LastName = "LastName"
                }
            };

            await _sut.Handle(command, CancellationToken.None);

            _bookRepositoryMock.Verify(x => x.Create(It.IsAny<Book>()), Times.Once);
        }
    }
}