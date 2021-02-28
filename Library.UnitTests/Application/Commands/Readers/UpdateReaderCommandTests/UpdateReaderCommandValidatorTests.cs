using FluentAssertions;
using Library.Application.Commands.Readers.UpdateReaderCommand;
using Xunit;

namespace Library.UnitTests.Application.Commands.Readers.UpdateReaderCommandTests
{
    public class UpdateReaderCommandValidatorTests
    {
        private UpdateReaderCommandValidator _sut => new UpdateReaderCommandValidator();

        [Fact]
        public void Validate_ValidCommand_NoErrors()
        {
            var command = new UpdateReaderCommand()
            {
                UserId = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                Password = "password",
                Email = "email@gmail.com"
            };

            var result = _sut.Validate(command);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}