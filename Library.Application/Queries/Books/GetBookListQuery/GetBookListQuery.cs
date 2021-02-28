using System.Collections.Generic;
using Library.Application.DTOs;
using MediatR;

namespace Library.Application.Queries.Books.GetBookListQuery
{
    public class GetBookListQuery : IRequest<List<BookListItemDTO>>
    {
        public string Title { get; set; }
        public AuthorDTO Author { get; set; }
        public int? YearPublished { get; set; }
        public string Description { get; set; }
        public bool? InStock { get; set; }
        public bool ShowReturnDueDate { get; set; }
    }
}