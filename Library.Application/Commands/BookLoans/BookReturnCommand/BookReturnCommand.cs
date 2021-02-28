using MediatR;

namespace Library.Application.Commands.BookLoans.BookReturnCommand
{
    public class BookReturnCommand : IRequest
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
    }
}