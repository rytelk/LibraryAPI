using Library.Application.DTOs;
using MediatR;

namespace Library.Application.Queries.Books.GetBookDetailsQuery
{
    public class GetBookDetailsQuery : IRequest<BookDetailsDTO>
    {
        public int BookId { get; set; }
        public bool ShowQueueDetails { get; set; }
    }
}