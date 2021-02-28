using Library.Application.DTOs;
using MediatR;

namespace Library.Application.Commands.Readers.CreateReaderCommand
{
    public class CreateReaderCommand : IRequest<UserDTO>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}