using Gastos_API.Data;
using Gastos_API.DTOs;
using Gastos_API.Models;
using Gastos_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Gastos_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IDespesaService _despesaService;
        private readonly IEntradaService _entradaService;

        public DashboardController(
            AppDbContext context,
            IDespesaService despesaService,
            IEntradaService entradaService
            )
        {
            _despesaService = despesaService;
            _entradaService = entradaService;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<DashboardDTO>> Get(Guid id)
        {
            var resumo = await _context.ResumoFinanceiroMensal
                .Where(x => x.UsuarioId == id)
                .ToListAsync();

            if (!resumo.Any())
            {
                var dashboardVazio = new DashboardDTO
                {
                    TotalDespesas = 0,
                    TotalEntradas = 0,
                    TotalSaldo = 0,
                    QuantidadeRegistro = 0,
                    Registros = new List<ResumoFinanceiroMensal>()
                };

                return Ok(dashboardVazio);
            }

            var resumoIds = resumo.Select(r => r.Id).ToList();

            var todasDespesas = await _context.DespesaItens.Where(d => resumoIds.Contains(d.DespesaId)).ToListAsync();
            var todasEntradas = await _context.EntradaItens.Where(d => resumoIds.Contains(d.Entrada_Id)).ToListAsync();

            var despesasPorResumo = todasDespesas.GroupBy(d => d.DespesaId).ToDictionary(g => g.Key, g => g.ToList());
            var entradasPorResumo = todasEntradas.GroupBy(d => d.Entrada_Id).ToDictionary(g => g.Key, g => g.ToList());

            var registros = resumo.Select(item => new ResumoFinanceiroMensal
            {
                UsuarioId = item.UsuarioId,
                Id = item.Id,
                ValorDespesaTotal = item.ValorDespesaTotal,
                ValorEntradaTotal = item.ValorEntradaTotal,
                DataInclusao = item.DataInclusao,
                Mes = item.Mes,
                Ano = item.Ano,

                ItensDespesa = despesasPorResumo.ContainsKey(item.Id)
                    ? despesasPorResumo[item.Id]
                    : new List<DespesaItem>(),

                ItensEntrada = entradasPorResumo.ContainsKey(item.Id)
                    ? entradasPorResumo[item.Id]
                    : new List<EntradaItem>()
                    }).ToList();

            var dashboard = new DashboardDTO
            {
                TotalDespesas = resumo.Sum(x => x.ValorDespesaTotal ?? 0),
                TotalEntradas = resumo.Sum(x => x.ValorEntradaTotal ?? 0),
                TotalSaldo = resumo.Sum(x => x.ValorEntradaTotal ?? 0)
                     - resumo.Sum(x => x.ValorDespesaTotal ?? 0),
                QuantidadeRegistro = resumo.Count,
                Registros = registros
            };

            return Ok(dashboard);
        }

    }
}
