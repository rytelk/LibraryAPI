using System.Threading.Tasks;
using Library.Domain.SeedWork;

namespace Library.Domain.AggregatesModel.BookAggregate
{
    public interface IBookRepository : IRepository<Book>
    {
        Book Create(Book book);
        Book Update(Book book);
        Task DeleteAsync(int id);
        Task<Book> GetAsync(int id);
    }
}