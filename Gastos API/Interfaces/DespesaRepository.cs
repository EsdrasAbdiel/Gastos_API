using Microsoft.EntityFrameworkCore;
using Gastos_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gastos_API.Data;
using Gastos_API.Services;

namespace Gastos_API.Repositorios
{
    public class DespesaRepository : IDespesaService
    {
        private readonly AppDbContext _context;

            public DespesaRepository(AppDbContext context)
            {
                _context = context;
            }
        public async Task<IEnumerable<Despesa>> BuscarTodasAsDespesasAsync(int ano)
        {
            return await _context.Despesas
                                 .Where(d =>  d.Ano == ano)
                                 .Select(d => new Despesa
                                 {
                                     Id = d.Id,
                                     Ano = d.Ano,
                                     Mes = d.Mes
                                 })
                                 .ToListAsync();
        }

        public async Task<Despesa?> BuscarDespesaPorIdAsync(Guid id)
        {
            return await _context.Despesas
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<List<DespesaItem>> BuscarItensDespesaPorIdAsync(Guid id)
        {
            return await _context.DespesaItens.Where(i => i.DespesaId == id).ToListAsync();

        }
        public async Task<Despesa> AdicionarDespesaAsync(Despesa despesa)
        {
            _context.Despesas.Add(despesa);
            await _context.SaveChangesAsync();
            return despesa;
        }

        public async Task AtualizarDespesaAsync(Despesa despesa)
        {
            _context.Despesas.Update(despesa);
            await _context.SaveChangesAsync();
        }

        public async Task DeletarDespesaAsync(Guid id)
        {
            var despesa = await _context.Despesas.FindAsync(id);
            if (despesa != null)
            {
                _context.Despesas.Remove(despesa);
                await _context.SaveChangesAsync();
            }
        }
    }
}
