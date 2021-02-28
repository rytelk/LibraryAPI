using Library.Application.DTOs;
using MediatR;

namespace Library.Application.Commands.Readers.UpdateReaderCommand
{
    public class UpdateReaderCommand : IRequest<UserDTO>
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}