using System.Threading;
using System.Threading.Tasks;
using Library.Application.Commands.BookLoans.BookLoanCommand;
using Library.Application.DTOs;
using Library.Domain.AggregatesModel.BookAggregate;
using Library.Domain.AggregatesModel.BookLoanAggregate;
using Library.Domain.AggregatesModel.UserAggregate;
using Library.Domain.SeedWork;
using Library.Domain.Services;
using Library.Infrastructure.Services;
using Moq;
using Xunit;

namespace Library.UnitTests.Application.Commands.BookLoans.BookLoanCommandTests
{
    public class BookLoanCommandHandlerTests
    {
        private readonly Mock<IBookLoanService> _bookLoanServiceMock;
        private readonly Mock<IBookLoanRepository> _bookLoanRepositoryMock;
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper<BookLoan, BookLoanInfoDTO>> _bookLoanMapperMock;

        private BookLoanCommandHandler _sut =>
            new BookLoanCommandHandler(_bookLoanServiceMock.Object, _userRepositoryMock.Object, _bookLoanRepositoryMock.Object, 
                _bookRepositoryMock.Object, _bookLoanMapperMock.Object);

        public BookLoanCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _bookLoanServiceMock = new Mock<IBookLoanService>();
            _bookLoanRepositoryMock = new Mock<IBookLoanRepository>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _bookLoanMapperMock = new Mock<IMapper<BookLoan, BookLoanInfoDTO>>();

            _bookLoanRepositoryMock.SetupGet(x => x.UnitOfWork)
                .Returns(_unitOfWorkMock.Object);
        }


        [Fact]
        public async Task Handle_ValidCommand_BookLoaned()
        {
            var bookId = 1;
            var userId = 1;
            var command = new BookLoanCommand()
            {
                BookId = bookId,
                UserId = userId
            };
            _userRepositoryMock.Setup(x => x.GetAsync(userId))
                .ReturnsAsync(
                    new User("FirstName", "LastName", new Email("email@gmail.com"), UserRolesConsts.Librarian));
            _bookRepositoryMock.Setup(x => x.GetAsync(bookId))
                .ReturnsAsync(new Book("Title", new Author("FirstName", "LastName"), 2020, "Description"));

            await _sut.Handle(command, CancellationToken.None);

            _bookLoanServiceMock.Verify(x => x.TryLoanBook(It.IsAny<BookLoan>()), Times.Once);
            _bookLoanRepositoryMock.Verify(x => x.Create(It.IsAny<BookLoan>()), Times.Once);
        }
    }
}