using Gastos_API.Data;
using Gastos_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Gastos_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarioController : ControllerBase
    {

        private readonly AppDbContext _context;

        public CalendarioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("listar/anos")]
        public async Task<ActionResult<IEnumerable<Ano>>> GetAnos()
        {
            var anos = Enumerable.Range(2025, 6)
                .Select((ano, index) => new Ano
                {
                    Id = ano,
                    AnoDescricao = ano
                })
                .ToList();

            return Ok(anos);
        }

        [HttpGet("listar/meses")]
        public async Task<ActionResult<IEnumerable<MesRelacionadoDespesas>>> GetMeses(int ano)
        {
            var language = new CultureInfo("pt-BR");

            var despesas = await _context.ResumoFinanceiroMensal.Where(d => d.Ano == ano).ToListAsync();

            var meses = Enumerable.Range(1, 12)
                .Select(m => new MesRelacionadoDespesas
                {
                    Id = m,
                    Nome = language.TextInfo.ToTitleCase(
                        language.DateTimeFormat.GetMonthName(m).ToLower()),
                    NomeAbreviado = language.TextInfo.ToTitleCase(
                        language.DateTimeFormat.GetAbbreviatedMonthName(m).ToLower()),
                    DespesaId = despesas.FirstOrDefault(d => d.Mes == m)?.Id
                })
                .ToList();

            return Ok(meses);
        } 
    }
}
 