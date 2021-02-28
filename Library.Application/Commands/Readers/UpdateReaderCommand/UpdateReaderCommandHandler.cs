using System.Threading;
using System.Threading.Tasks;
using Library.Application.DTOs;
using Library.Application.Services;
using Library.Domain.AggregatesModel.UserAggregate;
using Library.Domain.Exceptions;
using Library.Infrastructure.Services;
using MediatR;

namespace Library.Application.Commands.Readers.UpdateReaderCommand
{
    public class UpdateReaderCommandHandler : IRequestHandler<UpdateReaderCommand, UserDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHashService _passwordHashService;
        private readonly IMapper<User, UserDTO> _mapper;

        public UpdateReaderCommandHandler(IUserRepository userRepository, 
            IPasswordHashService passwordHashService,
            IMapper<User, UserDTO> mapper)
        {
            _userRepository = userRepository;
            _passwordHashService = passwordHashService;
            _mapper = mapper;
        }

        public async Task<UserDTO> Handle(UpdateReaderCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(command.UserId);
            if (user == null)
            {
                throw new UserNotFoundException(command.UserId);
            }
            if (user.Role == UserRolesConsts.Librarian)
            {
                throw new LibraryDomainException("Cannot modify user with role librarian.");
            }

            user.FirstName = command.FirstName;
            user.LastName = command.LastName;
            user.Email = new Email(command.Email);
            var (passwordHash, passwordSalt) = _passwordHashService.CreatePasswordHash(command.Password);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _userRepository.Update(user);
            await _userRepository.UnitOfWork.SaveChangesAsync();

            return _mapper.Map(user);
        }
    }
}