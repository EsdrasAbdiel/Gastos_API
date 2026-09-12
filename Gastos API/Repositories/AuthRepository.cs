using Gastos_API.Data;
using Gastos_API.Models;
using Gastos_API.Services;
using Microsoft.AspNetCore.Identity.Data;
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

        public async Task<Registro> AdicionarRegistroAsync(RegistroRequest registro)
        {
            var novoRegistro = new Registro
            {
                Id = Guid.NewGuid(),
                DataNascimento = registro.DataNascimento,
                Nome = registro.Nome,
                Email = registro.Email,
                Senha = registro.Senha,
                ConfirmarSenha = registro.ConfirmarSenha
            };

            _context.Registro.Add(novoRegistro);
            var retorno = await _context.SaveChangesAsync();

            if (retorno == 0)
                throw new Exception("Não foi possivel cadastrar o registro.");

            return novoRegistro;
        }

        public async Task<Registro?> BuscarUsuarioPeloEmailAsync(RegistroRequest registro)
        {
            return await _context.Registro
                .FirstOrDefaultAsync(u => u.Email == registro.Email);
        }

        public string GerarToken(Registro registro)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, registro.Email.ToString()),

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