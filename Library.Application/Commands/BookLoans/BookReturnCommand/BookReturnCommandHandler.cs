using System.Threading;
using System.Threading.Tasks;
using Library.Domain.Services;
using MediatR;

namespace Library.Application.Commands.BookLoans.BookReturnCommand
{
    public class BookReturnCommandHandler : IRequestHandler<BookReturnCommand>
    {
        private readonly IBookReturnService _bookReturnService;

        public BookReturnCommandHandler(IBookReturnService bookReturnService)
        {
            _bookReturnService = bookReturnService;
        }

        public async Task<Unit> Handle(BookReturnCommand command, CancellationToken cancellationToken)
        {
            await _bookReturnService.ReturnBook(command.UserId, command.BookId);
            return Unit.Value;
        }
    }
}