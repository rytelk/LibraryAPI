using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FluentAssertions;
using Library.Application.Commands.Books.CreateBookCommand;
using Library.Application.Commands.Books.UpdateBookCommand;
using Library.Application.DTOs;
using Library.FunctionalTests.Infrastructure;
using Xunit;

namespace Library.FunctionalTests
{
    [Collection("Sequential")]
    public class BooksControllerTests : TestBase
    {
        public BooksControllerTests(TestApplicationFactory<TestStartup> factory) : base(factory)
        {
        }
        
        [Fact]
        public async Task CreateBook_LibrarianUser_OkStatusCode()
        {
            // Arrange
            var claimsProvider = TestClaimsProvider.WithLibrarianClaims();
            var client = Factory.CreateClientWithTestAuth(claimsProvider);

            // Act
            var response = await CreateBook(client);

            // Assert
            response.Should().NotBeNull();
            response.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetBooks_LibrarianUser_OkStatusCode()
        {
            // Arrange
            var claimsProvider = TestClaimsProvider.WithLibrarianClaims();
            var client = Factory.CreateClientWithTestAuth(claimsProvider);
            await CreateBook(client);

            // Act
            var response = await client.GetAsync("api/books/");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetBook_LibrarianUser_OkStatusCode()
        {
            // Arrange
            var claimsProvider = TestClaimsProvider.WithLibrarianClaims();
            var client = Factory.CreateClientWithTestAuth(claimsProvider);
            var book = await CreateBook(client);

            // Act
            var response = await client.GetAsync($"api/books/{book.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateBook_LibrarianUser_OkStatusCode()
        {
            // Arrange
            var claimsProvider = TestClaimsProvider.WithLibrarianClaims();
            var client = Factory.CreateClientWithTestAuth(claimsProvider);
            var book = await CreateBook(client);
            var newTitle = "NewTitle";

            // Act
            var content = new UpdateBookCommand()
            {
                BookId = book.Id,
                Title = newTitle,
                Author = new AuthorDTO()
                {
                    FirstName = book.Author.FirstName,
                    LastName = book.Author.LastName
                },
                Description = book.Description,
                YearPublished = book.YearPublished
            };
            var response = await client.PutAsJsonAsync("api/books/", content);
            var responseBook = await response.Content.ReadFromJsonAsync<BookDTO>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseBook.Title.Should().Be(newTitle);
        }

        [Fact]
        public async Task DeleteBook_LibrarianUser_NoContentStatusCode()
        {
            // Arrange
            var claimsProvider = TestClaimsProvider.WithLibrarianClaims();
            var client = Factory.CreateClientWithTestAuth(claimsProvider);
            var book = await CreateBook(client);

            // Act
            var response = await client.DeleteAsync($"api/books/{book.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task BorrowBook_LibrarianUser_BookBorrowed()
        {
            // Arrange
            var claimsProvider = TestClaimsProvider.WithLibrarianClaims();
            var client = Factory.CreateClientWithTestAuth(claimsProvider);
            var book = await CreateBook(client);

            // Act
            var response = await client.PostAsync($"api/books/{book.Id}/borrow", null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task ReturnBook_LibrarianUser_BookReturned()
        {
            // Arrange
            var claimsProvider = TestClaimsProvider.WithLibrarianClaims();
            var client = Factory.CreateClientWithTestAuth(claimsProvider);
            var book = await CreateBook(client);
            var responseBorrow = await client.PostAsync($"api/books/{book.Id}/borrow", null);

            // Act
            var responseReturn = await client.PutAsync($"api/books/{book.Id}/return", null);

            // Assert
            responseReturn.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        private static async Task<BookDTO> CreateBook(HttpClient client)
        {
            var command = new CreateBookCommand()
            {
                Title = "Title",
                Author = new AuthorDTO()
                {
                    FirstName = "FirstName",
                    LastName = "LastName"
                },
                YearPublished = 2020,
                Description = "Description"
            };
            var response = await client.PostAsJsonAsync("api/books/", command);
            return await response.Content.ReadFromJsonAsync<BookDTO>();
        }
    }
}