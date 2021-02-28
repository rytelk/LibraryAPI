using FluentValidation;

namespace Library.Application.Commands.Readers.UpdateReaderCommand
{
    public class UpdateReaderCommandValidator : AbstractValidator<UpdateReaderCommand>
    {
        public UpdateReaderCommandValidator()
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