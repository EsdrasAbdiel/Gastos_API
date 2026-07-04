using Gastos_API.Enums;

namespace Gastos_API.Services
{
    public interface ICalendarioService
    {
        StatusCompetencia VerificarCompetenciaMesPeloAno(int ano, int competenciaMeses);
        StatusCompetencia VerificarStatusCompetenciaPeriodo(int competenciaAtual, int competenciaComparacao);

    }
}
