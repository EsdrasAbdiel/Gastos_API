using Microsoft.EntityFrameworkCore;
using Gastos_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gastos_API.Data;
using Gastos_API.Services;
using Microsoft.AspNetCore.Http.HttpResults;

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
                                 .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Despesa?> BuscarDespesaComItensPorIdAsync(Guid id) {             
            return await _context.Despesas
                                 .Include(d => d.ItensDespesa)
                                 .Include(d => d.ItensEntrada)
                                 .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Despesa?> BuscarEntradaComItensPorIdAsync(Guid id)
        {
            return await _context.Despesas
                                 .Include(d => d.ItensEntrada)
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

        public List<int> ObterIdsDosItensDespesasExistentes(IEnumerable<DespesaItem> itens)
        {
            return itens
                .Where(i => i.Id > 0)
                .Select(i => i.Id)
                .ToList();
        }

        public List<DespesaItem> ObterItensDespesasParaRemover(IEnumerable<DespesaItem> itensDoBanco, IEnumerable<int> idsDoFrontend)
        {
            return itensDoBanco
                .Where(db => !idsDoFrontend.Contains(db.Id))
                .ToList();
        }

        public void RemoverItensDespesaAsync(IEnumerable<DespesaItem> itensDespesa)
        {
            if (itensDespesa == null || !itensDespesa.Any())
                return;

            _context.DespesaItens.RemoveRange(itensDespesa);
        }

        public void RemoverDespesaAsync(Despesa despesa)
        {
            _context.Despesas.Remove(despesa);
        }

        public DespesaItem? ObterItemDespesaExistente(IEnumerable<DespesaItem> itensDoBanco,int idItem)
        {
            return itensDoBanco.FirstOrDefault(x => x.Id == idItem);
        }

        public async Task<DespesaItem> AdicionarNovaDespesaItemAsync(DespesaItem novaDespesaItem)
        {
            _context.DespesaItens.Add(novaDespesaItem);
            await _context.SaveChangesAsync();
            return novaDespesaItem;
        }

        public List<int> ObterIdsDosItensEntradasExistentes(IEnumerable<EntradaItem> itens)
        {
            return itens
                .Where(i => i.Id > 0)
                .Select(i => i.Id)
                .ToList();
        }

        public List<EntradaItem> ObterItensEntradasParaRemover(IEnumerable<EntradaItem> itensDoBanco, IEnumerable<int> idsDoFrontend)
        {
            return itensDoBanco
                .Where(db => !idsDoFrontend.Contains(db.Id))
                .ToList();
        }

        public void RemoverItensEntrada(IEnumerable<EntradaItem> itensEntrada)
        {
            if (itensEntrada == null || !itensEntrada.Any())
                return;

            _context.EntradaItens.RemoveRange(itensEntrada);
        }

        public EntradaItem? ObterItemEntradaExistente(IEnumerable<EntradaItem> itensDoBanco, int idItem)
        {
            return itensDoBanco.FirstOrDefault(x => x.Id == idItem);
        }

        public async Task<EntradaItem> AdicionarNovaEntradaItemAsync(EntradaItem novaEntradaItem)
        {
            _context.EntradaItens.Add(novaEntradaItem);
            await _context.SaveChangesAsync();
            return novaEntradaItem;
        }

        public async Task SalvarChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
