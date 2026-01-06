using Gastos_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gastos_API.Services
{
    public interface IDespesaService
    {
        Task<IEnumerable<Despesa>> BuscarTodasAsDespesasAsync(int ano);
        Task<Despesa?> BuscarDespesaPorIdAsync(Guid id);
        Task<List<DespesaItem>> BuscarItensDespesaPorIdAsync(Guid id);
        Task<Despesa> AdicionarDespesaAsync(Despesa despesa);
        Task AtualizarDespesaAsync(Despesa despesa);
        Task DeletarDespesaAsync(Guid id);
    }
}
