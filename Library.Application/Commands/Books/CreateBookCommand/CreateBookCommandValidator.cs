using FluentValidation;
using Library.Application.Validators;

namespace Library.Application.Commands.Books.CreateBookCommand
{
    public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
    {
        public CreateBookCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty();

            RuleFor(x => x.Author)
                .NotEmpty()
                .SetValidator(new AuthorValidator());

            RuleFor(x => x.Description)
                .NotEmpty();
        }
    }
}