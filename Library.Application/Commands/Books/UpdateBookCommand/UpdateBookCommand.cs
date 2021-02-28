using Library.Application.DTOs;
using MediatR;

namespace Library.Application.Commands.Books.UpdateBookCommand
{
    public class UpdateBookCommand : IRequest<BookDTO>
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public AuthorDTO Author { get; set; }
        public int YearPublished { get; set; }
        public string Description { get; set; }
    }
}