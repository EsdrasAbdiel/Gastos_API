using Microsoft.EntityFrameworkCore;
using Gastos_API.Models;
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

        public async Task<List<DespesaItem>> BuscarItensDespesaPorIdAsync(Guid id)
        {
            return await _context.DespesaItens.Where(i => i.DespesaId == id).ToListAsync();

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

    }
}
