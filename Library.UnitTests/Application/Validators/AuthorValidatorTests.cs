using System.Runtime.InteropServices;
using FluentAssertions;
using Library.Application.Commands.Readers.CreateReaderCommand;
using Library.Application.DTOs;
using Library.Application.Validators;
using Xunit;

namespace Library.UnitTests.Application.Validators
{
    public class AuthorValidatorTests
    {
        private AuthorValidator _sut => new AuthorValidator();

        [Fact]
        public void Validate_ValidAuthor_NoErrors()
        {
            var author = new AuthorDTO()
            {
                FirstName = "FirstName",
                LastName = "LastName"
            };

            var result = _sut.Validate(author);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("FirstName", "")]
        [InlineData("", "LastName")]
        public void Validate_InvalidAuthor_ContainsErrors(string firstName, string lastName)
        {
            var author = new AuthorDTO()
            {
                FirstName = firstName,
                LastName = lastName
            };

            var result = _sut.Validate(author);

            result.IsValid.Should().BeFalse();
        }
    }
}