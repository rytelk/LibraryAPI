using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Library.Application.DTOs;
using Library.Application.Queries.Books.GetBookListQuery;
using Library.Domain.AggregatesModel.BookAggregate;
using Library.Infrastructure;
using Moq;
using Xunit;

namespace Library.UnitTests.Application.Queries.Books
{
    public class GetBookListQueryHandlerTests
    {
        private readonly IBookListMapper _mapper;
        private readonly InMemoryLibraryContextFactory _libraryContextFactory;
        private readonly ILibraryContext _libraryContext;

        private GetBookListQueryHandler _sut => new GetBookListQueryHandler(_libraryContext, _mapper);

        public GetBookListQueryHandlerTests()
        {
            _mapper = new BookListMapper();
            _libraryContextFactory = new InMemoryLibraryContextFactory();
            _libraryContext = _libraryContextFactory.Create();
        }

        [Theory]
        [InlineData(null, null, null, null, null, null, 5)]
        [InlineData(null, "Harper", null, null, null, null, 2)]
        [InlineData("In Cold Blood", null, null, 1965, null, false, 0)]
        [InlineData(null, null, null, null, "The", null, 2)]
        [InlineData(null, null, "Capote", null, null, null, 1)]
        public async Task Handle_ValidQuery_BooksReturned(string title, string authorFirstName, string authorLastName, 
            int? yearPublished, string description, bool? inStock, int expectedCount)
        {
            await SeedTestData(_libraryContext);
            var query = new GetBookListQuery()
            {
                Title = title,
                Author = new AuthorDTO()
                {
                    FirstName = authorFirstName,
                    LastName = authorLastName
                },
                Description = description,
                YearPublished = yearPublished,
                InStock = inStock
            };

            var books = await _sut.Handle(query, CancellationToken.None);

            books.Count.Should().Be(expectedCount);
        }

        private async Task SeedTestData(ILibraryContext context)
        {
            context.Books.Add(new Book("To Kill a Mockingbird", new Author("Harper", "Lee"), 1960,
                "Beautiful story about how humans treat each other."));

            context.Books.Add(new Book("The Great Gatsby", new Author("Scott", "Fitzgerald"), 1925,
                "The greatest, most scathing dissection of the hollowness at the heart of the American dream."));

            context.Books.Add(new Book("One Hundred Years of Solitude", new Author("Gabriel", "Marquez"), 1813,
                "Both funny and moving."));

            context.Books.Add(new Book("In Cold Blood", new Author("Truman", "Capote"), 1965,
                "The true crime TV show."));

            context.Books.Add(new Book("Go Set a Watchman", new Author("Harper", "Lee"), 2015,
                "Stunning novel that is worth reading."));

            await context.SaveChangesAsync();
        }
    }
}