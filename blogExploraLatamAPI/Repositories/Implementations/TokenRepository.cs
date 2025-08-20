using blogExploraLatamAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace blogExploraLatamAPI.Repositories.Implementations
{
    public class TokenRepository : ITokenRepository
    {

        private readonly IConfiguration configuration;

        public TokenRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        public string CreateJwtToken(IdentityUser user, List<string> roles)
        {
            //Crear Claim, Definir claims básicos del usuario
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email)
            };

            //  Agregar roles como claims
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            //JWT Security Token Params, Obtener clave secreta desde appsettings.json
            // Clave secreta
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key no fue encontrada")
            ));

            // Crear credenciales de firma
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Configuracion  del token
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
