using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Library.Application.Queries.Books.GetBookDetailsQuery;
using Library.Application.Queries.Books.GetBookListQuery;
using Library.Domain.AggregatesModel.BookAggregate;
using Library.Domain.Exceptions;
using Library.Infrastructure;
using Xunit;

namespace Library.UnitTests.Application.Queries.Books
{
    public class GetBookDetailsQueryHandlerTests
    {
        private readonly IBookDetailsMapper _mapper;
        private readonly InMemoryLibraryContextFactory _libraryContextFactory;
        private readonly ILibraryContext _libraryContext;

        private GetBookDetailsQueryHandler _sut => new GetBookDetailsQueryHandler(_libraryContext, _mapper);

        public GetBookDetailsQueryHandlerTests()
        {
            _mapper = new BookDetailsMapper();
            _libraryContextFactory = new InMemoryLibraryContextFactory();
            _libraryContext = _libraryContextFactory.Create();
        }

        [Fact]
        public async Task Handle_BookExists_BookDetails()
        {
            // Arrange
            var book = new Book("Title", new Author("FirstName", "LastName"), 2020, "Description");
            _libraryContext.Books.Add(book);
            await _libraryContext.SaveChangesAsync();

            var query = new GetBookDetailsQuery()
            {
                BookId = book.Id
            };

            // Act
            var bookDetailsDto = await _sut.Handle(query, CancellationToken.None);

            // Assert
            bookDetailsDto.Should().NotBeNull();
            bookDetailsDto.Id.Should().Be(book.Id);
        }

        [Fact]
        public async Task Handle_BookNotExists_ExceptionThrown()
        {
            // Arrange
            var query = new GetBookDetailsQuery()
            {
                BookId = 1
            };

            // Act
            Func<Task> getBookDetails = async () => await _sut.Handle(query, CancellationToken.None);

            // Assert
            getBookDetails.Should().Throw<BookNotFoundException>();
        }
    }
}