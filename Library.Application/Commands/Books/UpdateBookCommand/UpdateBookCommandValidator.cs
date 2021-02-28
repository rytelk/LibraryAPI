using FluentValidation;
using Library.Application.Validators;

namespace Library.Application.Commands.Books.UpdateBookCommand
{
    public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
    {
        public UpdateBookCommandValidator()
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