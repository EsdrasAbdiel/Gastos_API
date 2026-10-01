using Gastos_API.Enums;
using Gastos_API.Models;
using Gastos_API.Services;
using System.Globalization;

namespace Gastos_API.Interfaces
{
    public class CalendarioRepository : ICalendarioService
    {
        private readonly IResumoFinanceiroMensalService _resumoFinanceiroMensalService;
        private readonly ICompetenciaService _competenciaService;

        public CalendarioRepository(
            IResumoFinanceiroMensalService resumoFinanceiroMensalService,
            ICompetenciaService competenciaService
            ) 
        {
            _resumoFinanceiroMensalService = resumoFinanceiroMensalService;
            _competenciaService = competenciaService;
        }

        public List<Ano> ListarAnosAsync()
        {
            var anoAtual = DateTime.Now.Year;
            return Enumerable.Range(2025, 6)
                .Select((ano, index) => new Ano
                {
                    Id = ano,
                    AnoDescricao = ano,
                    StatusCompetenciaAno = _competenciaService.VerificarStatusCompetenciaPeriodo(anoAtual, ano)
                })
                .ToList();
        }
        public async Task<List<MesRelacionadoDespesas>> ListarMesesComResumoFinanceirosAsync(int ano, Guid usuarioId)
        {
            var language = new CultureInfo("pt-BR");
            var resumoFinanceiro = await _resumoFinanceiroMensalService.BuscarResumoFinanceiroPeloAno(ano, usuarioId);

            return Enumerable.Range(1, 12)
                .Select(m => new MesRelacionadoDespesas
                {
                    Id = m,
                    Nome = language.TextInfo.ToTitleCase(
                        language.DateTimeFormat.GetMonthName(m).ToLower()),
                    NomeAbreviado = language.TextInfo.ToTitleCase(
                        language.DateTimeFormat.GetAbbreviatedMonthName(m).ToLower()),
                    DespesaId = resumoFinanceiro.FirstOrDefault(d => d.Mes == m)?.Id,
                    StatusCompetenciaMes = _competenciaService.VerificarCompetenciaMesPeloAno(ano, m),
                    ValorDespesaTotal = resumoFinanceiro.FirstOrDefault(d => d.Mes == m)?.ValorDespesaTotal ?? 0,
                    ValorReceitaTotal = resumoFinanceiro.FirstOrDefault(d => d.Mes == m)?.ValorEntradaTotal ?? 0
                })
                .ToList();
        }

        public List<Mes> ListarMesesAsync()
        {
            var language = new CultureInfo("pt-BR");

            return Enumerable.Range(1, 12)
                .Select(m => new Mes
                {
                    Id = m,
                    Nome = language.TextInfo.ToTitleCase(
                        language.DateTimeFormat.GetMonthName(m).ToLower()),
                    NomeAbreviado = language.TextInfo.ToTitleCase(
                        language.DateTimeFormat.GetAbbreviatedMonthName(m).ToLower())
                })
                .ToList();
        }
    }
}
