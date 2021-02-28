using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Library.Application.Commands.Accounts.LoginCommand;
using Library.Application.Commands.Accounts.RegisterCommand;
using Library.Application.DTOs;
using Library.FunctionalTests.Infrastructure;
using Xunit;

namespace Library.FunctionalTests
{
    [Collection("Sequential")]
    public class AccountsControllerTests : TestBase
    {
        public AccountsControllerTests(TestApplicationFactory<TestStartup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task RegisterAndLogin_AnonymousUser_JwtToken()
        {
            // Arrange
            var client = Factory.CreateClient();
            var email = "email@gmail.com";
            var password = "password";

            // Act
            var registerCommand = new RegisterCommand()
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "email@gmail.com",
                Password = "password",
                Role = UserRole.Librarian
            };
            var registerResponse = await client.PostAsJsonAsync("api/account/register", registerCommand);
            var userDto = registerResponse.Content.ReadFromJsonAsync<UserDTO>();

            var loginCommand = new LoginCommand()
            {
                Email = email,
                Password = password
            };
            var loginResponse = await client.PostAsJsonAsync("api/account/login", loginCommand);
            var jwtToken = registerResponse.Content.ReadFromJsonAsync<string>();

            // Assert
            registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            userDto.Should().NotBeNull();
            userDto.Id.Should().BeGreaterThan(0);

            loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            jwtToken.Should().NotBeNull();
        }
    }
}