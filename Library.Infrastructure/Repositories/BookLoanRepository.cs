using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Domain.AggregatesModel.BookLoanAggregate;
using Library.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories
{
    public class BookLoanRepository : IBookLoanRepository
    {
        private readonly ILibraryContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public BookLoanRepository(ILibraryContext context)
        {
            _context = context;
        }

        public BookLoan Create(BookLoan bookLoan)
        {
            if (bookLoan.IsTransient())
            {
                return _context.BookLoans
                    .Add(bookLoan)
                    .Entity;
            }
            else
            {
                return bookLoan;
            }
        }

        public BookLoan Update(BookLoan bookLoan)
        {
            return _context.BookLoans
                .Update(bookLoan)
                .Entity;
        }

        public async Task<List<BookLoan>> GetPendingBookLoans(int bookId)
        {
            return await _context.BookLoans
                .Include(x => x.Book)
                .Include(x => x.User)
                .Where(x => x.BookId == bookId && !x.BorrowedDate.HasValue && !x.ReturnedDate.HasValue)
                .ToListAsync();
        }
    }
}