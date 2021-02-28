using FluentValidation;

namespace Library.Application.Commands.Readers.CreateReaderCommand
{
    public class CreateReaderCommandValidator : AbstractValidator<CreateReaderCommand>
    {
        public CreateReaderCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty();

            RuleFor(x => x.LastName)
                .NotEmpty();

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}