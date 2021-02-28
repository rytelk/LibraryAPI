using System.Threading.Tasks;
using Library.Domain.AggregatesModel.BookAggregate;
using Library.Domain.SeedWork;

namespace Library.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ILibraryContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public BookRepository(ILibraryContext context)
        {
            _context = context;
        }

        public Book Create(Book book)
        {
            if (book.IsTransient())
            {
                return _context.Books
                    .Add(book)
                    .Entity;
            }
            else
            {
                return book;
            }
        }

        public Book Update(Book book)
        {
            return _context.Books
                .Update(book)
                .Entity;
        }

        public async Task DeleteAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            _context.Books.Remove(book);
        }

        public async Task<Book> GetAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }
    }
}