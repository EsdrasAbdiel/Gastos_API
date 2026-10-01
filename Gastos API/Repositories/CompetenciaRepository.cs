using Gastos_API.Enums;
using Gastos_API.Interfaces;
using Gastos_API.Services;

namespace Gastos_API.Repositories
{
    public class CompetenciaRepository : ICompetenciaService
    {
        public StatusCompetencia VerificarStatusCompetenciaPeriodo(int competenciaAtual, int competenciaComparacao) =>
    competenciaComparacao < competenciaAtual
        ? StatusCompetencia.Fechado
        : StatusCompetencia.Aberto;

        public StatusCompetencia VerificarCompetenciaMesPeloAno(int ano, int competenciaMeses)
        {
            var data = DateTime.Now;

            if (ano < data.Year)
                return StatusCompetencia.Fechado;

            if (ano > data.Year)
                return StatusCompetencia.Aberto;

            return VerificarStatusCompetenciaPeriodo(data.Month, competenciaMeses);
        }
    }
}
