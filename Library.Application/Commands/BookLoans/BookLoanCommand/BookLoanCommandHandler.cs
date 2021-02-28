using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Library.Application.DTOs;
using Library.Domain.AggregatesModel.BookAggregate;
using Library.Domain.AggregatesModel.BookLoanAggregate;
using Library.Domain.AggregatesModel.UserAggregate;
using Library.Domain.Exceptions;
using Library.Domain.Services;
using Library.Infrastructure.Services;
using MediatR;

namespace Library.Application.Commands.BookLoans.BookLoanCommand
{
    public class BookLoanCommandHandler : IRequestHandler<BookLoanCommand, BookLoanInfoDTO>
    {
        private readonly IBookLoanService _bookLoanService;
        private readonly IBookLoanRepository _bookLoanRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper<BookLoan, BookLoanInfoDTO> _bookLoanMapper;

        public BookLoanCommandHandler(IBookLoanService bookLoanService,
            IUserRepository userRepository,
            IBookLoanRepository bookLoanRepository,
            IBookRepository bookRepository,
            IMapper<BookLoan, BookLoanInfoDTO> bookLoanMapper)
        {
            _bookLoanService = bookLoanService;
            _userRepository = userRepository;
            _bookLoanRepository = bookLoanRepository;
            _bookLoanMapper = bookLoanMapper;
            _bookRepository = bookRepository;
        }

        public async Task<BookLoanInfoDTO> Handle(BookLoanCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(command.UserId);
            if (user == null)
            {
                throw new UserNotFoundException(command.UserId);
            }
            var book = await _bookRepository.GetAsync(command.BookId);
            if (book == null)
            {
                throw new BookNotFoundException(command.BookId);
            }
            if (user.BookLoans.Any(x => x.BookId == command.BookId && (x.IsBorrowed || x.IsPending)))
            {
                throw new LibraryDomainException(
                    $"User with id: {command.UserId} has already unfinished order for book with id: {command.BookId}");
            }

            var bookLoan = new BookLoan(command.BookId, command.UserId);
            await _bookLoanService.TryLoanBook(bookLoan);

            _bookLoanRepository.Create(bookLoan);
            await _bookLoanRepository.UnitOfWork.SaveChangesAsync();
            
            return _bookLoanMapper.Map(bookLoan);
        }
    }
}