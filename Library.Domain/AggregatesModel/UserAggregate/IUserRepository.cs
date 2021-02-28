using System.Threading.Tasks;
using Library.Domain.SeedWork;

namespace Library.Domain.AggregatesModel.UserAggregate
{
    public interface IUserRepository : IRepository<User>
    {
        User Create(User user);
        User Update(User user);
        Task DeleteAsync(int id);
        Task<User> GetAsync(int id);
        Task<User> GetAsync(string email);
    }
}