using System.Threading;
using System.Threading.Tasks;
using Library.Application.DomainEventHandlers;
using Library.Domain.AggregatesModel.BookAggregate;
using Library.Domain.AggregatesModel.BookLoanAggregate;
using Library.Domain.AggregatesModel.UserAggregate;
using Library.Domain.Events;
using Library.Domain.SeedWork;
using Library.Domain.Services;
using Moq;
using Xunit;

namespace Library.UnitTests.Application.DomainEventHandlers
{
    public class BookLoanFinishedEventHandlerTests
    {
        private readonly Mock<IBookLoanService> _bookLoanServiceMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly InMemoryLibraryContextFactory _libraryContextFactory = new InMemoryLibraryContextFactory();

        private BookLoanFinishedEventHandler _sut =>
            new BookLoanFinishedEventHandler(_bookLoanServiceMock.Object, _userRepositoryMock.Object);

        public BookLoanFinishedEventHandlerTests()
        {
            _bookLoanServiceMock = new Mock<IBookLoanService>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            var context = _libraryContextFactory.Create();
            var user = new User("FirstName", "LastName", new Email("email@gmail.com"), UserRolesConsts.Reader);
            context.Users.Add(user);
            context.SaveChanges();

            var bookLoan = new BookLoan(1, user.Id);
            context.BookLoans.Add(bookLoan);

            _userRepositoryMock.Setup(x => x.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(user);
            _userRepositoryMock.SetupGet(x => x.UnitOfWork)
                .Returns(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_CanLoanNextBook_BookLoaned()
        {
            // Arrange
            var userId = 1;
            var bookLoanFinishedEvent = new BookLoanFinishedEvent(userId);
            _bookLoanServiceMock.Setup(x => x.CanBeLoaned(It.IsAny<Book>(), It.IsAny<User>()))
                .Returns(true);

            // Act
            await _sut.Handle(bookLoanFinishedEvent, CancellationToken.None);

            // Assert
            _bookLoanServiceMock.Verify(x => x.TryLoanBook(It.IsAny<BookLoan>()), Times.Once);
        }

        [Fact]
        public async Task Handle_CannotLoanNextBook_BookNotLoaned()
        {
            // Arrange
            var userId = 1;
            var bookLoanFinishedEvent = new BookLoanFinishedEvent(userId);
            _bookLoanServiceMock.Setup(x => x.CanBeLoaned(It.IsAny<Book>(), It.IsAny<User>()))
                .Returns(false);

            // Act
            await _sut.Handle(bookLoanFinishedEvent, CancellationToken.None);

            // Assert
            _bookLoanServiceMock.Verify(x => x.TryLoanBook(It.IsAny<BookLoan>()), Times.Never);
        }
    }
}