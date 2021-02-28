using System.Threading.Tasks;
using FluentAssertions;
using Library.Domain.AggregatesModel.BookAggregate;
using Library.Domain.AggregatesModel.BookLoanAggregate;
using Library.Domain.AggregatesModel.UserAggregate;
using Library.Domain.SeedWork;
using Library.Domain.Services;
using Moq;
using Xunit;

namespace Library.UnitTests.Domain.Services
{
    public class BookLoanServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly InMemoryLibraryContextFactory _inMemoryLibraryContextFactory;

        public BookLoanServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _inMemoryLibraryContextFactory = new InMemoryLibraryContextFactory();
        }

        private BookLoanService _sut => new BookLoanService(_userRepositoryMock.Object, _bookRepositoryMock.Object);

        [Fact]
        public async Task TryLoanBook_BookInStock_BookLoaned()
        {
            // Arrange
            var context = _inMemoryLibraryContextFactory.Create();
            var user = new User("FirstName", "LastName", new Email("email@gmail.com"), UserRolesConsts.Reader);
            context.Users.Add(user);
            context.SaveChanges();

            var book = new Book("Title", new Author("FirstName", "LastName"), 2020, "Description");
            context.Books.Add(book);
            context.SaveChanges();

            var bookLoan = new BookLoan(book.Id, user.Id);
            context.BookLoans.Add(bookLoan);

            _userRepositoryMock.Setup(x => x.GetAsync(user.Id)).ReturnsAsync(user);
            _bookRepositoryMock.Setup(x => x.GetAsync(book.Id)).ReturnsAsync(book);

            // Act
            var result = await _sut.TryLoanBook(bookLoan);

            // Assert
            result.Should().BeTrue();
            bookLoan.IsBorrowed.Should().BeTrue();
            book.InStock.Should().BeFalse();
        }

        [Fact]
        public async Task TryLoanBook_BookNotInStock_BookLoaned()
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

            _userRepositoryMock.Setup(x => x.GetAsync(user.Id)).ReturnsAsync(user);
            _bookRepositoryMock.Setup(x => x.GetAsync(book.Id)).ReturnsAsync(book);

            // Act
            var result = await _sut.TryLoanBook(bookLoan);

            // Assert
            result.Should().BeFalse();
            bookLoan.IsPending.Should().BeTrue();
        }
    }
}