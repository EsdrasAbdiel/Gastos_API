using Gastos_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Runtime.Serialization;

namespace Gastos_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
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
        public ActionResult<IEnumerable<Mes>> GetMeses()
        {
            var language = new CultureInfo("pt-BR");

            var meses = Enumerable.Range(1, 12)
                .Select(m => new Mes
                {
                    Id = m,
                    Nome = language.TextInfo.ToTitleCase(
                        language.DateTimeFormat.GetMonthName(m).ToLower()),
                    NomeAbreviado = language.TextInfo.ToTitleCase(
                        language.DateTimeFormat.GetAbbreviatedMonthName(m).ToLower()),
                })
                .ToArray();

            return Ok(meses);
        } 
    }
}
 