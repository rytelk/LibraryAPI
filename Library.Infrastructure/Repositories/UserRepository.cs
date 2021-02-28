using System.Threading.Tasks;
using Library.Domain.AggregatesModel.UserAggregate;
using Library.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ILibraryContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public UserRepository(ILibraryContext context)
        {
            _context = context;
        }

        public User Create(User user)
        {
            if (user.IsTransient())
            {
                return _context.Users
                    .Add(user)
                    .Entity;
            }
            else
            {
                return user;
            }
        }

        public User Update(User user)
        {
            return _context.Users
                .Update(user)
                .Entity;
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
        }

        public async Task<User> GetAsync(int id)
        {
            return await _context.Users
                .Include(x => x.BookLoans)
                    .ThenInclude(x => x.Book)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> GetAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email.EmailAddress == email);
        }
    }
}