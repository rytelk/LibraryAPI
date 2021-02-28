using FluentValidation;
using Library.Application.DTOs;

namespace Library.Application.Validators
{
    public class AuthorValidator : AbstractValidator<AuthorDTO>
    {
        public AuthorValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty();

            RuleFor(x => x.LastName)
                .NotEmpty();
        }
    }
}