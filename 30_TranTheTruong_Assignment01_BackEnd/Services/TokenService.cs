using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace _30_TranTheTruong_Assignment01_BackEnd.Services
{
    public interface ITokenService
    {
        string CreateToken(AuthenticatedUser user);
    }

    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string CreateToken(AuthenticatedUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var minutes = int.TryParse(_config["Jwt:ExpireMinutes"], out var m) ? m : 120;

            var claims = new List<Claim>
        {
            new("sub", user.Id.ToString()),
            new("name", user.Name),
            new("email", user.Email),
            new("role", user.Role)
        };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(minutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
