using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Library.Application.DomainEventHandlers;
using Library.Domain.AggregatesModel.BookLoanAggregate;
using Library.Domain.AggregatesModel.UserAggregate;
using Library.Domain.Events;
using Library.Domain.SeedWork;
using Library.Domain.Services;
using Moq;
using Xunit;

namespace Library.UnitTests.Application.DomainEventHandlers
{
    public class BookReturnedEventHandlerTests
    {
        private readonly Mock<IBookLoanRepository> _bookLoanRepositoryMock;
        private readonly Mock<IBookLoanService> _bookLoanServiceMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        private BookReturnedEventHandler _sut =>
            new BookReturnedEventHandler(_bookLoanRepositoryMock.Object, _bookLoanServiceMock.Object);

        public BookReturnedEventHandlerTests()
        {
            _bookLoanRepositoryMock = new Mock<IBookLoanRepository>();
            _bookLoanServiceMock = new Mock<IBookLoanService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _bookLoanRepositoryMock.SetupGet(x => x.UnitOfWork)
                .Returns(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_PendingBookLoanExists_BorrowBook()
        {
            // Arrange
            var bookId = 1;
            var userId = 1;
            var bookReturnedEvent = new BookReturnedEvent(bookId);
            _bookLoanRepositoryMock.Setup(x => x.GetPendingBookLoans(bookId))
                .ReturnsAsync(new List<BookLoan>()
                {
                    new BookLoan(bookId, userId)
                });

            // Act
            await _sut.Handle(bookReturnedEvent, CancellationToken.None);

            // Assert
            _bookLoanServiceMock.Verify(x => x.TryLoanBook(It.IsAny<BookLoan>()), Times.Once);
        }

        [Fact]
        public async Task Handle_PendingBookLoanNotExists_BookNotBorrowed()
        {
            // Arrange
            var bookId = 1;
            var bookReturnedEvent = new BookReturnedEvent(bookId);
            _bookLoanRepositoryMock.Setup(x => x.GetPendingBookLoans(bookId))
                .ReturnsAsync(new List<BookLoan>());

            // Act
            await _sut.Handle(bookReturnedEvent, CancellationToken.None);

            // Assert
            _bookLoanServiceMock.Verify(x => x.TryLoanBook(It.IsAny<BookLoan>()), Times.Never);
        }
    }
}