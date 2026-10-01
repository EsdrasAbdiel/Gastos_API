using Gastos_API.Models;

namespace Gastos_API.Services
{
    public interface IResumoFinanceiroMensalService
    {
        Task<IEnumerable<ResumoFinanceiroMensal>> ListarResumosFinanceiroAsync(int ano);
        Task<ResumoFinanceiroMensal?> BuscarDespesaComItensPorIdAsync(Guid id);
        Task<ResumoFinanceiroMensal?> BuscarResumoFinanceiroPorIdAsync(Guid id);
        Task<ResumoFinanceiroMensal> CadastrarResumoFinanceiroAsync(ResumoFinanceiroMensalRequest request);
        Task<ResumoFinanceiroMensal?> BuscarEntradaComItensPorIdAsync(Guid id);
        Task AtualizarDespesaAsync(ResumoFinanceiroMensal despesa);
        void RemoverDespesaAsync(ResumoFinanceiroMensal despesa);
        Task SalvarChangesAsync();
        Task DeletarDespesaAsync(Guid id);
        Task<ResumoFinanceiroMensal?> BuscarPorAnoEMes(int ano, int mes, Guid usuarioId);
        Task<List<ResumoFinanceiroMensal>> BuscarResumoFinanceiroPeloAno(int ano, Guid usuarioId);
        Task<List<ResumoFinanceiroMensal>> ListarResumoFinanceiroPorUsuarioId(Guid id);
        Task<ResumoFinanceiroMensal> CadastrarResumoFinanceiroImportacaoAsync(ResumoFinanceiroMensal novoResumo);
        Task RemoverDespesaAsync(Guid id);
    }
}
