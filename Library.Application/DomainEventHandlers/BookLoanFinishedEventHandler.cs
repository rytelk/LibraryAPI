using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Library.Domain.AggregatesModel.UserAggregate;
using Library.Domain.Events;
using Library.Domain.Services;
using MediatR;

namespace Library.Application.DomainEventHandlers
{
    public class BookLoanFinishedEventHandler : INotificationHandler<BookLoanFinishedEvent>
    {
        private readonly IBookLoanService _bookLoanService;
        private readonly IUserRepository _userRepository;

        public BookLoanFinishedEventHandler(IBookLoanService bookLoanService, IUserRepository userRepository)
        {
            _bookLoanService = bookLoanService;
            _userRepository = userRepository;
        }

        public async Task Handle(BookLoanFinishedEvent loanFinishedEvent, CancellationToken cancellationToken)
        {
            // If user had 3 books and just returned one, we have to try loan him any of ordered by him books that is available. 
            var user = await _userRepository.GetAsync(loanFinishedEvent.UserId);
            var pendingUserBookLoans = user.BookLoans.Where(x => x.IsPending);
            
            var bookLoan = pendingUserBookLoans.FirstOrDefault(x => _bookLoanService.CanBeLoaned(x.Book, user));
            if (bookLoan != null)
            {
                await _bookLoanService.TryLoanBook(bookLoan);
            }

            await _userRepository.UnitOfWork.SaveChangesAsync();
        }
    }
}