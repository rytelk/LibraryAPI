using Library.Application.DTOs;
using Library.Domain.AggregatesModel.UserAggregate;
using Library.Infrastructure.Services;

namespace Library.Application.Mappers
{
    public class UserMapper : IMapper<User, UserDTO>
    {
        public UserDTO Map(User source)
        {
            return new UserDTO()
            {
                Id = source.Id,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email.EmailAddress
            };
        }
    }
}