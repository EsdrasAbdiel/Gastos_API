using Gastos_API.Data;
using Gastos_API.Models;
using Gastos_API.Services;

namespace Gastos_API.Repositorios
{
    public class AuthRepository : IAuthService
    {
        private readonly AppDbContext _context;

        public AuthRepository(AppDbContext context) { 
            _context = context;
        }

        public async Task<Registro> AdicionarRegistroAsync(Registro registro)
        {
            _context.Registro.Add(registro);
            await _context.SaveChangesAsync();
            return registro;
        }
    }
}
