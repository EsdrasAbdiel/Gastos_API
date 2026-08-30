using Gastos_API.Data;
using Gastos_API.Models;
using Gastos_API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Gastos_API.Repositorios
{
    public class AuthRepository : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthRepository(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<Registro> AdicionarRegistroAsync(Registro registro)
        {
            _context.Registro.Add(registro);
            await _context.SaveChangesAsync();
            return registro;
        }

        public async Task<Registro?> BuscarUsuarioPeloEmailAsync(string email)
        {
            return await _context.Registro
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public string GerarToken(Registro registro)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, registro.Id.ToString()),

                    new Claim(ClaimTypes.Email, registro.Email)
                }),

                Expires = DateTime.UtcNow.AddHours(2),

                Issuer = _configuration["Jwt:Issuer"],

                Audience = _configuration["Jwt:Audience"],

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}