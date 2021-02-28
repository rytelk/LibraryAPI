using System.Threading;
using System.Threading.Tasks;
using Library.Domain.AggregatesModel.BookAggregate;
using Library.Domain.Exceptions;
using MediatR;

namespace Library.Application.Commands.Books.DeleteBookCommand
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand>
    {
        private readonly IBookRepository _bookRepository;

        public DeleteBookCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<Unit> Handle(DeleteBookCommand command, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetAsync(command.BookId);
            if (book == null)
            {
                throw new BookNotFoundException(command.BookId);
            }

            if (!book.InStock)
            {
                throw new LibraryDomainException($"Book with id: {command.BookId} cannot be deleted, because it is not in the stock.");
            }

            await _bookRepository.DeleteAsync(book.Id);
            await _bookRepository.UnitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}