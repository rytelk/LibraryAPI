using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Domain.SeedWork;

namespace Library.Domain.AggregatesModel.BookLoanAggregate
{
    public interface IBookLoanRepository : IRepository<BookLoan>
    {
        BookLoan Create(BookLoan bookLoan);
        BookLoan Update(BookLoan bookLoan);
        Task<List<BookLoan>> GetPendingBookLoans(int bookId);
    }
}