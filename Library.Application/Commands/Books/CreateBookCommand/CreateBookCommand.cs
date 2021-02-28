using Library.Application.DTOs;
using MediatR;

namespace Library.Application.Commands.Books.CreateBookCommand
{
    public class CreateBookCommand : IRequest<BookDTO>
    {
        public string Title { get; set; }
        public AuthorDTO Author { get; set; }
        public int YearPublished { get; set; }
        public string Description { get; set; }
    }
}