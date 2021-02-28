using System.Collections.Generic;
using System.Linq;
using Library.Application.DTOs;
using Library.Domain.AggregatesModel.BookAggregate;
using Library.Domain.AggregatesModel.BookLoanAggregate;

namespace Library.Application.Queries.Books.GetBookDetailsQuery
{
    public interface IBookDetailsMapper
    {
        BookDetailsDTO Map(Book book, bool showQueueDetails);
    }

    public class BookDetailsMapper : IBookDetailsMapper
    {
        public BookDetailsDTO Map(Book book, bool showQueueDetails)
        {
            return new BookDetailsDTO()
            {
                Id = book.Id,
                Title = book.Title,
                Author = new AuthorDTO()
                {
                    FirstName = book.Author.FirstName,
                    LastName = book.Author.LastName
                },
                YearPublished = book.YearPublished,
                Description = book.Description,
                InStock = book.InStock,
                BookLoanQueue = MapLoanQueue(book, showQueueDetails)
            };
        }

        private BookLoanQueueDTO MapLoanQueue(Book book, bool showQueueDetails)
        {
            var readersLendPending = book.BookLoans.Where(x => x.IsPending).ToList();

            return new BookLoanQueueDTO()
            {
                QueueLength = readersLendPending.Count(),
                Readers = showQueueDetails ? MapReaders(readersLendPending) : null
            };
        }

        private List<UserDTO> MapReaders(IEnumerable<BookLoan> readersLendPending)
        {
            return readersLendPending.Select(x => new UserDTO()
                {
                    Id = x.UserId,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    Email = x.User.Email.EmailAddress
                })
                .ToList();
        }
    }
}