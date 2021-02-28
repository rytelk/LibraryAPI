using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Library.Domain.AggregatesModel.UserAggregate;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Library.Infrastructure.Services
{
    public interface IJwtTokenService
    {
        string CreateUserToken(User user);
    }

    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly IConfiguration _configuration;

        public JwtTokenService(JwtSecurityTokenHandler tokenHandler, IConfiguration configuration)
        {
            _tokenHandler = tokenHandler;
            _configuration = configuration;
        }


        public string CreateUserToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = GetTokenDescriptor(user);
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private SecurityTokenDescriptor GetTokenDescriptor(User user)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSecret"]);
            return new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                IssuedAt = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
        }
    }
}