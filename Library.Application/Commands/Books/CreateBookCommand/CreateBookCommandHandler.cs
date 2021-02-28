using System.Threading;
using System.Threading.Tasks;
using Library.Application.DTOs;
using Library.Domain.AggregatesModel.BookAggregate;
using Library.Infrastructure.Services;
using MediatR;

namespace Library.Application.Commands.Books.CreateBookCommand
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, BookDTO>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper<Book, BookDTO> _bookMapper;

        public CreateBookCommandHandler(IBookRepository bookRepository, IMapper<Book, BookDTO> bookMapper)
        {
            _bookRepository = bookRepository;
            _bookMapper = bookMapper;
        }

        public async Task<BookDTO> Handle(CreateBookCommand command, CancellationToken cancellationToken)
        {
            var author = new Author(command.Author.FirstName, command.Author.LastName);
            var book = new Book(command.Title, author, command.YearPublished, command.Description);

            book = _bookRepository.Create(book);
            await _bookRepository.UnitOfWork.SaveChangesAsync();

            return _bookMapper.Map(book);
        }
    }
}