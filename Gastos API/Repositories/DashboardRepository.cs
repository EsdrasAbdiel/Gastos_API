using Gastos_API.DTOs;
using Gastos_API.Models;
using Gastos_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gastos_API.Repositories
{
    public class DashboardRepository : IDashboardService
    {
        private readonly IResumoFinanceiroMensalService _resumoFinanceiroMensalService;
        private readonly IDashboardService _dashboardService;
        private readonly IDespesaService _despesaService;
        private readonly IEntradaService _entradaService;
        public DashboardRepository(
            IResumoFinanceiroMensalService resumoFinanceiroMensalService,
            IDashboardService dashboardService,
            IDespesaService despesaService,
            IEntradaService entradaService
            )
        {
            _resumoFinanceiroMensalService = resumoFinanceiroMensalService;
            _dashboardService = dashboardService;
            _despesaService = despesaService;
            _entradaService = entradaService;
        }

        public async Task<Dashboard> ListarAsync(Guid id)
        {
            var resumosFinanceirosMensais = await _resumoFinanceiroMensalService.ListarResumoFinanceiroPorUsuarioId(id);

            var ids = resumosFinanceirosMensais.Select(r => r.Id).ToList();

            var todasDespesas = await _despesaService.ListarDespesasPorIdsAsync(ids);
            var todasEntradas = await _entradaService.ListarEntradasPorIdsAsync(ids);

            var despesasPorResumoFinanceiro = todasDespesas.GroupBy(d => d.DespesaId).ToDictionary(g => g.Key, g => g.ToList());
            var entradasPorResumoFinanceiro = todasEntradas.GroupBy(e => e.Entrada_Id).ToDictionary(g => g.Key, g => g.ToList());

            var registros = MontarRegistrosParaDashboardAsync(resumosFinanceirosMensais, despesasPorResumoFinanceiro, entradasPorResumoFinanceiro);

            var dashboard = new Dashboard
            {
                TotalDespesas = resumosFinanceirosMensais.Sum(x => x.ValorDespesaTotal ?? 0),
                TotalEntradas = resumosFinanceirosMensais.Sum(x => x.ValorEntradaTotal ?? 0),
                TotalSaldo = resumosFinanceirosMensais.Sum(x => x.ValorEntradaTotal ?? 0)
                     - resumosFinanceirosMensais.Sum(x => x.ValorDespesaTotal ?? 0),
                QuantidadeRegistro = resumosFinanceirosMensais.Count,
                Registros = registros
            };

            return dashboard;
        }

        private static List<ResumoFinanceiroMensal> MontarRegistrosParaDashboardAsync(List<ResumoFinanceiroMensal> resumosFinanceirosMensais, Dictionary<Guid, List<DespesaItem>> despesasPorResumoFinanceiro, Dictionary<Guid, List<EntradaItem>> entradasPorResumoFinanceiro)
        {
            return resumosFinanceirosMensais.Select(item => new ResumoFinanceiroMensal
            {
                UsuarioId = item.UsuarioId,
                Id = item.Id,
                ValorDespesaTotal = item.ValorDespesaTotal,
                ValorEntradaTotal = item.ValorEntradaTotal,
                DataInclusao = item.DataInclusao,
                Mes = item.Mes,
                Ano = item.Ano,
                ItensDespesa = despesasPorResumoFinanceiro.TryGetValue(item.Id, out List<DespesaItem>? despesas) ? despesas : [],
                ItensEntrada = entradasPorResumoFinanceiro.TryGetValue(item.Id, out List<EntradaItem>? entradas) ? entradas : []
            }).ToList();
        }
    }
}
