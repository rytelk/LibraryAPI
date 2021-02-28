using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Library.Domain.AggregatesModel.BookLoanAggregate;
using Library.Domain.Events;
using Library.Domain.Services;
using MediatR;

namespace Library.Application.DomainEventHandlers
{
    public class BookReturnedEventHandler : INotificationHandler<BookReturnedEvent>
    {
        private readonly IBookLoanRepository _bookLoanRepository;
        private readonly IBookLoanService _bookLoanService;

        public BookReturnedEventHandler(IBookLoanRepository bookLoanRepository, 
            IBookLoanService bookLoanService)
        {
            _bookLoanRepository = bookLoanRepository;
            _bookLoanService = bookLoanService;
        }

        public async Task Handle(BookReturnedEvent bookReturnedEvent, CancellationToken cancellationToken)
        {
            // Try borrow book to someone from the queue
            var pendingBookLoans = await _bookLoanRepository.GetPendingBookLoans(bookReturnedEvent.BookId);

            if(pendingBookLoans.Any())
            {
                var bookLoan = pendingBookLoans.OrderBy(x => x.CreatedDate)
                    .First();
                await _bookLoanService.TryLoanBook(bookLoan);
            }

            await _bookLoanRepository.UnitOfWork.SaveChangesAsync();
        }
    }
}