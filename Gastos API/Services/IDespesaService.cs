using Gastos_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gastos_API.Services
{
    public interface IDespesaService
    {
        Task<List<DespesaItem>> BuscarItensDespesaPorIdAsync(Guid id);
        List<int> ObterIdsDosItensDespesasExistentes(IEnumerable<DespesaItem> itens);
        List<DespesaItem> ObterItensDespesasParaRemover(IEnumerable<DespesaItem> itensDoBanco, IEnumerable<int> idsDoFrontend);
        void RemoverItensDespesaAsync(IEnumerable<DespesaItem> itensDespesa);
        DespesaItem? ObterItemDespesaExistente(IEnumerable<DespesaItem> itensDoBanco, int idItem);
        Task<DespesaItem> AdicionarNovaDespesaItemAsync(DespesaItem novaDespesaItem);
    }
}
