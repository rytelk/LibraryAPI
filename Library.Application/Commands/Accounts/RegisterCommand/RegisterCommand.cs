using Library.Application.DTOs;
using MediatR;

namespace Library.Application.Commands.Accounts.RegisterCommand
{
    public class RegisterCommand : IRequest<UserDTO>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole? Role { get; set; }
    }
}