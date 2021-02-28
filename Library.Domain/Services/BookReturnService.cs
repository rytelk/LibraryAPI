using System.Linq;
using System.Threading.Tasks;
using Library.Domain.AggregatesModel.BookAggregate;
using Library.Domain.AggregatesModel.BookLoanAggregate;
using Library.Domain.AggregatesModel.UserAggregate;
using Library.Domain.Exceptions;

namespace Library.Domain.Services
{
    public interface IBookReturnService
    {
        Task ReturnBook(int userId, int bookId);
    }

    public class BookReturnService : IBookReturnService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBookRepository _bookRepository;

        public BookReturnService(IUserRepository userRepository, 
            IBookRepository bookRepository)
        {
            _userRepository = userRepository;
            _bookRepository = bookRepository;
        }


        public async Task ReturnBook(int userId, int bookId)
        {
            var user = await _userRepository.GetAsync(userId);
            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }
            var bookLoan = user.BookLoans.FirstOrDefault(x => x.BookId == bookId && x.IsBorrowed);
            if (bookLoan == null || !bookLoan.IsBorrowed)
            {
                throw new LibraryDomainException(
                    $"User with id: {user.Id} does not have book with id: {bookId}");
            }

            bookLoan.FinishLoan();
            var book = await _bookRepository.GetAsync(bookId);
            book.Return();

            await _bookRepository.UnitOfWork.SaveChangesAsync();
        }
    }
}