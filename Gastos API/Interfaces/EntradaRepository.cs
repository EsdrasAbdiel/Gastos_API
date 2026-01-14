using Gastos_API.Data;
using Gastos_API.Services;
using Gastos_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Gastos_API.Interfaces
{
    public class EntradaRepository : IEntradaService
    {
        private readonly AppDbContext _context;

        public EntradaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<EntradaItem>> BuscarItensEntradaPorIdAsync(Guid id)
        {
            return await _context.EntradaItens.Where(e => e.Entrada_Id == id).ToListAsync();
        }
    }
}
