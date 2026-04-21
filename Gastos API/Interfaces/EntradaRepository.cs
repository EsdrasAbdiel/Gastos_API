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
    }
}
