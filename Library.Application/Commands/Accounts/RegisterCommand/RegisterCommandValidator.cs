using FluentValidation;

namespace Library.Application.Commands.Accounts.RegisterCommand
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
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

            RuleFor(x => x.Role)
                .NotEmpty();
        }
    }
}