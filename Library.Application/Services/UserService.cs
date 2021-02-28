using System.Threading.Tasks;
using Library.Application.Models;
using Library.Domain.AggregatesModel.UserAggregate;
using Library.Domain.Exceptions;
using Library.Infrastructure.Services;

namespace Library.Application.Services
{
    public interface IUserService
    {
        Task<string> Login(string email, string password);
        Task<User> Register(RegisterUserModel userModel);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHashService _passwordHashService;
        private readonly IJwtTokenService _jwtTokenService;

        public UserService(IUserRepository userRepository, 
            IPasswordHashService passwordHashService,
            IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _passwordHashService = passwordHashService;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<string> Login(string email, string password)
        {
            var user = await _userRepository.GetAsync(email);
            if (user == null || !_passwordHashService.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                throw new InvalidUserCredentialsException();

            return _jwtTokenService.CreateUserToken(user);
        }

        public async Task<User> Register(RegisterUserModel userModel)
        {
            var existingUser = await _userRepository.GetAsync(userModel.Email);
            if (existingUser != null)
                throw new LibraryDomainException($"User with email {userModel.Email} already exists.");

            var user = new User(userModel.FirstName, userModel.LastName, new Email(userModel.Email), userModel.Role);
            var (passwordHash, passwordSalt) = _passwordHashService.CreatePasswordHash(userModel.Password);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _userRepository.Create(user);
            await _userRepository.UnitOfWork.SaveChangesAsync();

            return user;
        }
    }
}