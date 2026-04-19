using Gastos_API.Data;
using Gastos_API.DTOs;
using Gastos_API.Models;
using Gastos_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                return NotFound("Nenhum registro encontrado para este usuário.");

            var registros = new List<ResumoFinanceiroMensal>();

            foreach (var item in resumo)
            {
                var despesas = await _despesaService.BuscarItensDespesaPorIdAsync(item.Id);
                var entradas = await _entradaService.BuscarItensEntradaPorIdAsync(item.Id);

                var itemRegistro = new ResumoFinanceiroMensal
                {
                    UsuarioId = item.UsuarioId,
                    Id = item.Id,
                    ValorDespesaTotal = item.ValorDespesaTotal,
                    ValorEntradaTotal = item.ValorEntradaTotal,
                    DataInclusao = item.DataInclusao,
                    Mes = item.Mes,
                    Ano = item.Ano,
                    ItensDespesa = despesas,
                    ItensEntrada = entradas
                };

                registros.Add(itemRegistro);
            }

            var dashboard = new DashboardDTO
            {
                TotalDespesas = resumo.Sum(x => x.ValorDespesaTotal ?? 0),
                TotalEntradas = resumo.Sum(x => x.ValorEntradaTotal ?? 0),
                TotalSaldo = resumo.Sum(x => x.ValorEntradaTotal ?? 0) - resumo.Sum(x => x.ValorDespesaTotal ?? 0),
                QuantidadeRegistro = resumo.Count,
                Registros = registros
            };

            return Ok(dashboard);
        }

    }
}
