using System.Threading;
using System.Threading.Tasks;
using Library.Application.DTOs;
using Library.Application.Models;
using Library.Application.Services;
using Library.Domain.AggregatesModel.UserAggregate;
using Library.Infrastructure.Services;
using MediatR;

namespace Library.Application.Commands.Accounts.RegisterCommand
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, UserDTO>
    {
        private readonly IUserService _userService;
        private readonly IMapper<User, UserDTO> _mapper;

        public RegisterCommandHandler(IUserService userService, IMapper<User, UserDTO> mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<UserDTO> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = await _userService.Register(new RegisterUserModel()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = request.Password,
                Role = request.Role.ToString()
            });

            return _mapper.Map(user);
        }
    }
}