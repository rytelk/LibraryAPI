using FluentAssertions;
using Library.Application.Commands.Books.UpdateBookCommand;
using Library.Application.DTOs;
using Xunit;

namespace Library.UnitTests.Application.Commands.Books.UpdateBookCommandTests
{
    public class UpdateBookCommandValidatorTests
    {
        private UpdateBookCommandValidator _sut => new UpdateBookCommandValidator();

        [Fact]
        public void Validate_ValidCommand_NoErrors()
        {
            var command = new UpdateBookCommand()
            {
                BookId = 1,
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