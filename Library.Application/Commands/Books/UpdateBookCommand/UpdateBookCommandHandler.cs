using System.Threading;
using System.Threading.Tasks;
using Library.Application.DTOs;
using Library.Domain.AggregatesModel.BookAggregate;
using Library.Domain.Exceptions;
using Library.Infrastructure.Services;
using MediatR;

namespace Library.Application.Commands.Books.UpdateBookCommand
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, BookDTO>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper<Book, BookDTO> _bookMapper;

        public UpdateBookCommandHandler(IBookRepository bookRepository, IMapper<Book, BookDTO> bookMapper)
        {
            _bookRepository = bookRepository;
            _bookMapper = bookMapper;
        }

        public async Task<BookDTO> Handle(UpdateBookCommand command, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetAsync(command.BookId);
            if (book == null)
            {
                throw new BookNotFoundException(command.BookId);
            }

            book.Title = command.Title;
            book.Author = new Author(command.Author.FirstName, command.Author.LastName);
            book.YearPublished = command.YearPublished;
            book.Description = command.Description;

            book = _bookRepository.Update(book);
            await _bookRepository.UnitOfWork.SaveChangesAsync();

            return _bookMapper.Map(book);
        }
    }
}