using Gastos_API.Models;

namespace Gastos_API.Services
{
    public interface IEntradaService
    {
        Task<List<EntradaItem>> BuscarItensEntradaPorIdAsync(Guid id);
        List<int> ObterIdsDosItensEntradasExistentes(IEnumerable<EntradaItem> itens);
        List<EntradaItem> ObterItensEntradasParaRemover(IEnumerable<EntradaItem> itensDoBanco, IEnumerable<int> idsDoFrontend);
        void RemoverItensEntrada(IEnumerable<EntradaItem> itensEntrada);
        EntradaItem? ObterItemEntradaExistente(IEnumerable<EntradaItem> itensDoBanco, int idItem);
        Task<EntradaItem> AdicionarNovaEntradaItemAsync(EntradaItem novaEntradaItem);
    }
}
