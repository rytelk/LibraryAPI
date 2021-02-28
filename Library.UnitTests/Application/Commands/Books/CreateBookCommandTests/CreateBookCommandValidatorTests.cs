using FluentAssertions;
using Library.Application.Commands.Books.CreateBookCommand;
using Library.Application.DTOs;
using Xunit;

namespace Library.UnitTests.Application.Commands.Books.CreateBookCommandTests
{
    public class CreateBookCommandValidatorTests
    {
        private CreateBookCommandValidator _sut => new CreateBookCommandValidator();

        [Fact]
        public void Validate_ValidCommand_NoErrors()
        {
            var command = new CreateBookCommand()
            {
                Title = "Title",
                Description = "Description",
                YearPublished = 2020,
                Author = new AuthorDTO()
                {
                    FirstName = "FirstName",
                    LastName = "LastName"
                }
            };

            var result = _sut.Validate(command);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}