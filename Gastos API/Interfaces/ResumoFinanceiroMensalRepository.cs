using Gastos_API.Data;
using Gastos_API.Models;
using Microsoft.EntityFrameworkCore;
using Gastos_API.Services;

namespace Gastos_API.Interfaces
{
    public class ResumoFinanceiroMensalRepository : IResumoFinanceiroMensalService
    {
        private readonly AppDbContext _context;

        public ResumoFinanceiroMensalRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ResumoFinanceiroMensal>> BuscarTodasAsDespesasAsync(int ano)
        {
            return await _context.ResumoFinanceiroMensal
                                 .Where(d => d.Ano == ano)
                                 .Select(d => new ResumoFinanceiroMensal
                                 {
                                     Id = d.Id,
                                     Ano = d.Ano,
                                     Mes = d.Mes
                                 })
                                 .ToListAsync();
        }

        public async Task<ResumoFinanceiroMensal?> BuscarDespesaPorIdAsync(Guid id)
        {
            return await _context.ResumoFinanceiroMensal
                                 .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<ResumoFinanceiroMensal?> BuscarDespesaComItensPorIdAsync(Guid id)
        {
            return await _context.ResumoFinanceiroMensal
                                 .Include(d => d.ItensDespesa)
                                 .Include(d => d.ItensEntrada)
                                 .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<ResumoFinanceiroMensal?> BuscarEntradaComItensPorIdAsync(Guid id)
        {
            return await _context.ResumoFinanceiroMensal
                                 .Include(d => d.ItensEntrada)
                                 .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<ResumoFinanceiroMensal> AdicionarDespesaAsync(ResumoFinanceiroMensal despesa)
        {
            _context.ResumoFinanceiroMensal.Add(despesa);
            await _context.SaveChangesAsync();
            return despesa;
        }

        public async Task AtualizarDespesaAsync(ResumoFinanceiroMensal despesa)
        {
            _context.ResumoFinanceiroMensal.Update(despesa);
            await _context.SaveChangesAsync();
        }

        public void RemoverDespesaAsync(ResumoFinanceiroMensal despesa)
        {
            _context.ResumoFinanceiroMensal.Remove(despesa);
        }

        public async Task SalvarChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeletarDespesaAsync(Guid id)
        {
            var despesa = await _context.ResumoFinanceiroMensal.FindAsync(id);
            if (despesa != null)
            {
                _context.ResumoFinanceiroMensal.Remove(despesa);
                await _context.SaveChangesAsync();
            }
        }
    }
}
