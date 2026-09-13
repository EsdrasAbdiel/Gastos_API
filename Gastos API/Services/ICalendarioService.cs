using Gastos_API.Enums;
using Gastos_API.Models;

namespace Gastos_API.Services
{
    public interface ICalendarioService
    {
        StatusCompetencia VerificarCompetenciaMesPeloAno(int ano, int competenciaMeses);
        StatusCompetencia VerificarStatusCompetenciaPeriodo(int competenciaAtual, int competenciaComparacao);
        List<Ano> ListarAnosAsync();
        Task<List<MesRelacionadoDespesas>> ListarMesesComResumoFinanceirosAsync(int ano, Guid usuarioId);
        List<Mes> ListarMesesAsync();
    }
}
