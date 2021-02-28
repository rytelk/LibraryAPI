using Library.Application.DTOs;
using Library.Domain.AggregatesModel.BookAggregate;
using Library.Infrastructure.Services;

namespace Library.Application.Mappers
{
    public class BookMapper : IMapper<Book, BookDTO>
    {
        public BookDTO Map(Book source)
        {
            return new BookDTO()
            {
                Id = source.Id,
                Title = source.Title,
                Author = new AuthorDTO()
                {
                    FirstName = source.Author.FirstName,
                    LastName = source.Author.LastName
                },
                Description = source.Description,
                YearPublished = source.YearPublished,
                InStock = source.InStock
            };
        }
    }
}