using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Library.API.Infrastructure.Extensions;
using Library.Application.Commands.Readers.CreateReaderCommand;
using Library.Application.Commands.Readers.UpdateReaderCommand;
using Library.FunctionalTests.Infrastructure;
using Xunit;
using System.Net.Http.Json;
using Library.Application.DTOs;

namespace Library.FunctionalTests
{
    [Collection("Sequential")]
    public class ReadersControllerTests : TestBase
    {
        public ReadersControllerTests(TestApplicationFactory<TestStartup> factory) : base(factory)
        {
        }

        [Theory]
        [InlineData("api/readers/")]
        [InlineData("api/readers/1")]
        public async Task GetReaders_ReaderUser_Forbidden(string url)
        {
            // Arrange
            var claimsProvider = TestClaimsProvider.WithReaderClaims();
            var client = Factory.CreateClientWithTestAuth(claimsProvider);

            // Act
            var response = await client.GetAsync(url);            

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task CreateReader_ReaderUser_Forbidden()
        {
            // Arrange
            var claimsProvider = TestClaimsProvider.WithReaderClaims();
            var client = Factory.CreateClientWithTestAuth(claimsProvider);

            // Act
            var content = new CreateReaderCommand()
                {
                    FirstName = "FirstName",
                    LastName = "LastName",
                    Email = "email@email.com",
                    Password = "password"
                };
            var response = await client.PostAsJsonAsync("api/readers/", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task UpdateReader_ReaderUser_Forbidden()
        {
            // Arrange
            var claimsProvider = TestClaimsProvider.WithReaderClaims();
            var client = Factory.CreateClientWithTestAuth(claimsProvider);

            // Act
            var content = new UpdateReaderCommand()
            {
                UserId = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "email@email.com",
                Password = "password"
            };
            var response = await client.PutAsJsonAsync("api/readers/", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task CreateReader_LibrarianUser_OkStatusCode()
        {
            // Arrange
            var claimsProvider = TestClaimsProvider.WithLibrarianClaims();
            var client = Factory.CreateClientWithTestAuth(claimsProvider);

            // Act
            var response = await CreateReader(client);

            // Assert
            response.Should().NotBeNull();
            response.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetReaders_LibrarianUser_OkStatusCode()
        {
            // Arrange
            var claimsProvider = TestClaimsProvider.WithLibrarianClaims();
            var client = Factory.CreateClientWithTestAuth(claimsProvider);

            // Act
            var response = await client.GetAsync("api/readers/");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetReader_LibrarianUser_OkStatusCode()
        {
            // Arrange
            var claimsProvider = TestClaimsProvider.WithLibrarianClaims();
            var client = Factory.CreateClientWithTestAuth(claimsProvider);
            var reader = await CreateReader(client);

            // Act
            var response = await client.GetAsync($"api/readers/{reader.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateReader_LibrarianUser_OkStatusCode()
        {
            // Arrange
            var claimsProvider = TestClaimsProvider.WithLibrarianClaims();
            var client = Factory.CreateClientWithTestAuth(claimsProvider);
            var reader = await CreateReader(client);
            var newName = "FirstName2";

            // Act
            var content = new UpdateReaderCommand()
            {
                UserId = reader.Id,
                FirstName = newName,
                LastName = "LastName2",
                Email = "email@email.com",
                Password = "password"
            };
            var response = await client.PutAsJsonAsync("api/readers/", content);
            var responseUser = await response.Content.ReadFromJsonAsync<UserDTO>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseUser.FirstName.Should().Be(newName);
        }

        [Fact]
        public async Task DeleteReader_LibrarianUser_NoContentStatusCode()
        {
            // Arrange
            var claimsProvider = TestClaimsProvider.WithLibrarianClaims();
            var client = Factory.CreateClientWithTestAuth(claimsProvider);
            var reader = await CreateReader(client);

            // Act
            var response = await client.DeleteAsync($"api/readers/{reader.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        private static async Task<UserDTO> CreateReader(HttpClient client)
        {
            var command = new CreateReaderCommand()
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "email@email.com",
                Password = "password"
            };
            var response = await client.PostAsJsonAsync("api/readers/", command);
            return await response.Content.ReadFromJsonAsync<UserDTO>();
        }
    }
}