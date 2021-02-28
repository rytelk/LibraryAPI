using System.Linq;

namespace Library.Infrastructure.Services
{
    public interface IPasswordHashService
    {
        (byte[] PasswordHash, byte[] PasswordSalt) CreatePasswordHash(string password);
        bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt);
    }

    public class PasswordHashService : IPasswordHashService
    {
        public (byte[] PasswordHash, byte[] PasswordSalt) CreatePasswordHash(string password)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            var passwordSalt = hmac.Key;
            var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return (passwordHash, passwordSalt);
        }

        public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(storedHash);
        }
    }
}