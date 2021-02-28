using System.Linq;
using System.Threading.Tasks;
using Library.Domain.AggregatesModel.BookAggregate;
using Library.Domain.AggregatesModel.BookLoanAggregate;
using Library.Domain.AggregatesModel.UserAggregate;
using Library.Domain.Exceptions;

namespace Library.Domain.Services
{
    public interface IBookLoanService
    {
        Task<bool> TryLoanBook(BookLoan bookLoan);
        bool CanBeLoaned(Book book, User user);
    }

    public class BookLoanService : IBookLoanService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBookRepository _bookRepository;

        private const int BorrowedBooksLimit = 3;

        public BookLoanService(IUserRepository userRepository,
            IBookRepository bookRepository)
        {
            _userRepository = userRepository;
            _bookRepository = bookRepository;
        }

        public async Task<bool> TryLoanBook(BookLoan bookLoan)
        {
            var user = await _userRepository.GetAsync(bookLoan.UserId);
            var book = await _bookRepository.GetAsync(bookLoan.BookId);

            if (CanBeLoaned(book, user))
            {
                book.Borrow();
                bookLoan.SetBookBorrowed();
                return true;
            }

            return false;
        }

        public bool CanBeLoaned(Book book, User user)
        {
            return book.InStock && user.BookLoans.Count(x => x.IsBorrowed) < BorrowedBooksLimit;
        }
    }
}