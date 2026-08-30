using Gastos_API.Models;

namespace Gastos_API.Services
{
    public interface IResumoFinanceiroMensalService
    {
        Task<IEnumerable<ResumoFinanceiroMensal>> BuscarTodasAsDespesasAsync(int ano);
        Task<ResumoFinanceiroMensal?> BuscarDespesaComItensPorIdAsync(Guid id);
        Task<ResumoFinanceiroMensal?> BuscarDespesaPorIdAsync(Guid id);
        Task<ResumoFinanceiroMensal> AdicionarDespesaAsync(ResumoFinanceiroMensal despesa);
        Task<ResumoFinanceiroMensal?> BuscarEntradaComItensPorIdAsync(Guid id);
        Task AtualizarDespesaAsync(ResumoFinanceiroMensal despesa);
        void RemoverDespesaAsync(ResumoFinanceiroMensal despesa);
        Task SalvarChangesAsync();
        Task DeletarDespesaAsync(Guid id);
        Task<ResumoFinanceiroMensal?> BuscarPorAnoEMes(int ano, int mes, Guid usuarioId);
    }
}
