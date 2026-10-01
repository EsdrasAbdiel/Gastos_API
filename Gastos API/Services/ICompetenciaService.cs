using Gastos_API.Enums;

namespace Gastos_API.Services
{
    public interface ICompetenciaService
    {
        public StatusCompetencia VerificarStatusCompetenciaPeriodo(int competenciaAtual, int competenciaComparacao);
        public StatusCompetencia VerificarCompetenciaMesPeloAno(int ano, int competenciaMeses);
    }
}
