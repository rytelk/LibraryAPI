using System;
using System.Threading.Tasks;
using FluentAssertions;
using Library.Domain.AggregatesModel.BookAggregate;
using Library.Domain.AggregatesModel.BookLoanAggregate;
using Library.Domain.AggregatesModel.UserAggregate;
using Library.Domain.Exceptions;
using Library.Domain.SeedWork;
using Library.Domain.Services;
using Moq;
using Xunit;

namespace Library.UnitTests.Domain.Services
{
    public class BookReturnServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly InMemoryLibraryContextFactory _inMemoryLibraryContextFactory;

        public BookReturnServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _inMemoryLibraryContextFactory = new InMemoryLibraryContextFactory();
        }

        private BookReturnService _sut => new BookReturnService(_userRepositoryMock.Object, _bookRepositoryMock.Object);

        [Fact]
        public async Task ReturnBook_BorrowedBook_BookReturned()
        {
            // Arrange
            var context = _inMemoryLibraryContextFactory.Create();
            var user = new User("FirstName", "LastName", new Email("email@gmail.com"), UserRolesConsts.Reader);
            context.Users.Add(user);
            context.SaveChanges();

            var book = new Book("Title", new Author("FirstName", "LastName"), 2020, "Description");
            book.Borrow();
            context.Books.Add(book);
            context.SaveChanges();

            var bookLoan = new BookLoan(book.Id, user.Id);
            context.BookLoans.Add(bookLoan);
            bookLoan.SetBookBorrowed();

            _userRepositoryMock.Setup(x => x.GetAsync(user.Id)).ReturnsAsync(user);
            _bookRepositoryMock.Setup(x => x.GetAsync(book.Id)).ReturnsAsync(book);
            _bookRepositoryMock.SetupGet(x => x.UnitOfWork).Returns(_unitOfWorkMock.Object);

            // Act
             await _sut.ReturnBook(user.Id, book.Id);

            // Assert
            bookLoan.IsReturned.Should().BeTrue();
            book.InStock.Should().BeTrue();
        }

        [Fact]
        public void ReturnBook_UserNotExists_UserNotFoundExceptionThrown()
        {
            var userId = 1;
            _userRepositoryMock.Setup(x => x.GetAsync(userId)).ReturnsAsync((User)null);

            Func<Task> returnBook = async () => await _sut.ReturnBook(userId, 1);

            returnBook.Should().Throw<UserNotFoundException>();
        }

        [Fact]
        public void ReturnBook_UserDontHaveBook_ExceptionThrown()
        {
            // Arrange
            var context = _inMemoryLibraryContextFactory.Create();
            var user = new User("FirstName", "LastName", new Email("email@gmail.com"), UserRolesConsts.Reader);
            context.Users.Add(user);
            context.SaveChanges();

            _userRepositoryMock.Setup(x => x.GetAsync(user.Id)).ReturnsAsync(user);

            // Act
            Func<Task> returnBook = async () => await _sut.ReturnBook(user.Id, 1);

            // Assert
            returnBook.Should().Throw<LibraryDomainException>();
        }
    }
}