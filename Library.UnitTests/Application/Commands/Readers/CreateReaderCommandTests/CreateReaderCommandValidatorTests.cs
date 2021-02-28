using FluentAssertions;
using Library.Application.Commands.Readers.CreateReaderCommand;
using Xunit;

namespace Library.UnitTests.Application.Commands.Readers.CreateReaderCommandTests
{
    public class CreateReaderCommandValidatorTests
    {
        private CreateReaderCommandValidator _sut => new CreateReaderCommandValidator();

        [Fact]
        public void Validate_ValidCommand_NoErrors()
        {
            var command = new CreateReaderCommand()
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Password = "password",
                Email = "email@gmail.com"
            };

            var result = _sut.Validate(command);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Validate_EmptyName_ContainError()
        {
            var command = new CreateReaderCommand()
            {
                FirstName = "",
                LastName = "LastName",
                Password = "password",
                Email = "email@gmail.com"
            };

            var result = _sut.Validate(command);

            result.IsValid.Should().BeFalse();
        }
    }
}