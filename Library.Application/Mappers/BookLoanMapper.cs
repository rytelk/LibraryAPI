using Library.Application.DTOs;
using Library.Domain.AggregatesModel.BookLoanAggregate;
using Library.Infrastructure.Services;

namespace Library.Application.Mappers
{
    public class BookLoanMapper : IMapper<BookLoan, BookLoanInfoDTO>
    {
        public BookLoanInfoDTO Map(BookLoan source)
        {
            return new BookLoanInfoDTO()
            {
                BookId = source.BookId,
                UserId = source.UserId,
                CreatedDate = source.CreatedDate,
                BorrowedDate = source.BorrowedDate,
                ReturnDueDate = source.ReturnDueDate,
                ReturnedDate = source.ReturnedDate,
                BookLoanStatus = GetBookLoanStatus(source)
            };
        }

        private BookLoanStatusDTO GetBookLoanStatus(BookLoan source)
        {
            if (source.IsBorrowed)
            {
                return BookLoanStatusDTO.Borrowed;
            }
            else if (source.IsReturned)
            {
                return BookLoanStatusDTO.Returned;
            }
            else
            {
                return BookLoanStatusDTO.Pending;
            }
        }
    }
}