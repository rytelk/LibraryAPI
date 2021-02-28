using System.Threading;
using System.Threading.Tasks;
using Library.Application.Services;
using MediatR;

namespace Library.Application.Commands.Accounts.LoginCommand
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly IUserService _userService;

        public LoginCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return await _userService.Login(request.Email, request.Password);
        }
    }
}