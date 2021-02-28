using System.Linq;
using FluentAssertions;
using Library.Application.Commands.Accounts.RegisterCommand;
using Xunit;

namespace Library.UnitTests.Application.Commands.Accounts.RegisterCommandTests
{
    public class RegisterCommandValidatorTests
    {
        private RegisterCommandValidator _sut => new RegisterCommandValidator();

        [Fact]
        public void Validate_ValidRegisterCommand_NoErrors()
        {
            var command = new RegisterCommand()
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "email@gmail.com",
                Password = "password",
                Role = UserRole.Librarian
            };

            var result = _sut.Validate(command);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Validate_InvalidEmail_EmailError()
        {
            var command = new RegisterCommand()
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "invalidemail",
                Password = "password",
                Role = UserRole.Librarian
            };

            var result = _sut.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            var error = result.Errors.First();
            error.PropertyName = nameof(RegisterCommand.Email);
        }
    }
}