using Gastos_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gastos_API.Services
{
    public interface IDespesaService
    {
        Task<IEnumerable<Despesa>> BuscarTodasAsDespesasAsync(int ano);
        Task<Despesa?> BuscarDespesaComItensPorIdAsync(Guid id);
        Task<Despesa?> BuscarDespesaPorIdAsync(Guid id);
        Task<List<DespesaItem>> BuscarItensDespesaPorIdAsync(Guid id);
        Task<Despesa> AdicionarDespesaAsync(Despesa despesa);
        Task AtualizarDespesaAsync(Despesa despesa);
        Task DeletarDespesaAsync(Guid id);
        List<int> ObterIdsDosItensDespesasExistentes(IEnumerable<DespesaItem> itens);
        List<DespesaItem> ObterItensDespesasParaRemover(IEnumerable<DespesaItem> itensDoBanco, IEnumerable<int> idsDoFrontend);
        void RemoverItensDespesaAsync(IEnumerable<DespesaItem> itensDespesa);
        void RemoverDespesaAsync(Despesa despesa);
        DespesaItem? ObterItemDespesaExistente(IEnumerable<DespesaItem> itensDoBanco, int idItem);
        Task<DespesaItem> AdicionarNovaDespesaItemAsync(DespesaItem novaDespesaItem);
        Task<Despesa?> BuscarEntradaComItensPorIdAsync(Guid id);
        List<int> ObterIdsDosItensEntradasExistentes(IEnumerable<EntradaItem> itens);
        List<EntradaItem> ObterItensEntradasParaRemover(IEnumerable<EntradaItem> itensDoBanco, IEnumerable<int> idsDoFrontend);
        void RemoverItensEntrada(IEnumerable<EntradaItem> itensEntrada);
        EntradaItem? ObterItemEntradaExistente(IEnumerable<EntradaItem> itensDoBanco, int idItem);
        Task<EntradaItem> AdicionarNovaEntradaItemAsync(EntradaItem novaEntradaItem);
        Task SalvarChangesAsync();
    }
}
