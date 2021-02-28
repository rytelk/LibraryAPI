using MediatR;

namespace Library.Application.Commands.Books.DeleteBookCommand
{
    public class DeleteBookCommand : IRequest
    {
        public int BookId { get; set; }
    }
}