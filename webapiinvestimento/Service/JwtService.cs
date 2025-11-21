using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace webapiinvestimento.Services
{
    public interface IJwtService
    {
        string GerarToken(string usuario, string role = "cliente");
        DateTime ObterDataExpiracao();
    }

    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GerarToken(string usuario, string role = "cliente")
        {
            var jwtSection = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expireMinutes = int.Parse(jwtSection["ExpireMinutes"] ?? "60");
            var expiration = DateTime.UtcNow.AddMinutes(expireMinutes);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario),
                new Claim(ClaimTypes.Name, usuario),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public DateTime ObterDataExpiracao()
        {
            var jwtSection = _configuration.GetSection("Jwt");
            var expireMinutes = int.Parse(jwtSection["ExpireMinutes"] ?? "60");
            return DateTime.UtcNow.AddMinutes(expireMinutes);
        }
    }
}
