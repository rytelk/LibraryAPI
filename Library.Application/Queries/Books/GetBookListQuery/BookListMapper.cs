using System.Collections.Generic;
using System.Linq;
using Library.Application.DTOs;
using Library.Domain.AggregatesModel.BookAggregate;

namespace Library.Application.Queries.Books.GetBookListQuery
{
    public interface IBookListMapper
    {
        List<BookListItemDTO> MapBookList(List<Book> books, bool showDueReturnDate);
    }

    public class BookListMapper : IBookListMapper
    {
        public List<BookListItemDTO> MapBookList(List<Book> books, bool showDueReturnDate)
        {
            return books.Select(x => MapBook(x, showDueReturnDate))
                .ToList();
        }

        public BookListItemDTO MapBook(Book source, bool showDueReturnDate)
        {
            return new BookListItemDTO()
            {
                Id = source.Id,
                Title = source.Title,
                Author = source.Author.GetFullName(),
                InStock = source.InStock,
                ReturnDueDate = showDueReturnDate ? source.GetReturnDueDate() : null
            };
        }
    }
}