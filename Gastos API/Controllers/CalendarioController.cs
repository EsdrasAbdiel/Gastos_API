using Gastos_API.Data;
using Gastos_API.Enums;
using Gastos_API.Interfaces;
using Gastos_API.Models;
using Gastos_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Gastos_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarioController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly ICalendarioService _calendarioService;

        public CalendarioController(AppDbContext context, ICalendarioService calendarioService, ILogger<CalendarioController> logger)
        {
            _context = context;
            _calendarioService = calendarioService;
        }

        [HttpGet("listar/anos")]
        public async Task<ActionResult<IEnumerable<Ano>>> GetAnos()
        {
            var anoAtual = DateTime.Now.Year;
            var anos = Enumerable.Range(2025, 6)
                .Select((ano, index) => new Ano
                {
                    Id = ano,
                    AnoDescricao = ano,
                    StatusCompetenciaAno = _calendarioService.VerificarStatusCompetenciaPeriodo(anoAtual, ano)
                })
                .ToList();

            return Ok(anos);
        }

        [HttpGet("listar/meses")]
        public async Task<ActionResult<IEnumerable<MesRelacionadoDespesas>>> GetMeses(int ano, Guid usuarioId)
        {
            var language = new CultureInfo("pt-BR");

            var despesas = await _context.ResumoFinanceiroMensal.Where(d => d.UsuarioId == usuarioId && d.Ano == ano).ToListAsync();

            Console.WriteLine(despesas);

            var meses = Enumerable.Range(1, 12)
                .Select(m => new MesRelacionadoDespesas
                {
                    Id = m,
                    Nome = language.TextInfo.ToTitleCase(
                        language.DateTimeFormat.GetMonthName(m).ToLower()),
                    NomeAbreviado = language.TextInfo.ToTitleCase(
                        language.DateTimeFormat.GetAbbreviatedMonthName(m).ToLower()),
                    DespesaId = despesas.FirstOrDefault(d => d.Mes == m)?.Id,
                    StatusCompetenciaMes = _calendarioService.VerificarCompetenciaMesPeloAno(ano, m)
                })
                .ToList();

            return Ok(meses);
        }

        [HttpGet("dashboard/meses")]
        public ActionResult<IEnumerable<Mes>> GetMesesDashboard()
        {
            var language = new CultureInfo("pt-BR");

            var meses = Enumerable.Range(1, 12)
                .Select(m => new Mes
                {
                    Id = m,
                    Nome = language.TextInfo.ToTitleCase(
                        language.DateTimeFormat.GetMonthName(m).ToLower()),
                    NomeAbreviado = language.TextInfo.ToTitleCase(
                        language.DateTimeFormat.GetAbbreviatedMonthName(m).ToLower())
                })
                .ToList();

            return Ok(meses);
        }
    }
}
 