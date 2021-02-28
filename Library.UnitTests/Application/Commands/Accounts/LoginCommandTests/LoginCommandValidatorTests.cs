using FluentAssertions;
using Library.Application.Commands.Accounts.LoginCommand;
using Xunit;

namespace Library.UnitTests.Application.Commands.Accounts.LoginCommandTests
{
    public class LoginCommandValidatorTests
    {
        private LoginCommandValidator _sut => new LoginCommandValidator();

        [Fact]
        public void Validate_ValidLoginCommand_NoErrors()
        {
            var command = new LoginCommand()
            {
                Email = "email@gmail.com",
                Password = "password"
            };

            var result = _sut.Validate(command);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Theory]
        [InlineData("invalidemail", "password")]
        [InlineData("", "password")]
        [InlineData("valid@email.com", "")]
        public void Validate_InvalidCommand_ContainsError(string email, string password)
        {
            var command = new LoginCommand()
            {
                Email = email,
                Password = password
            };

            var result = _sut.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
        }
    }
}