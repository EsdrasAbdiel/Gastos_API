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
        public ActionResult<IEnumerable<Ano>> ListarAnosAsync()
        {
            var retorno = _calendarioService.ListarAnosAsync();

            return Ok(retorno);
        }

        [HttpGet("listar/mesesComResumoFinanceiro")]
        public async Task<ActionResult<IEnumerable<MesRelacionadoDespesas>>> ListarMesesComResumoFinanceirosAsync(int ano, Guid usuarioId)
        {
            var retorno = await _calendarioService.ListarMesesComResumoFinanceirosAsync(ano, usuarioId);

            return Ok(retorno);
        }

        [HttpGet("listar/meses")]
        public ActionResult<IEnumerable<Mes>> ListarMesesAsync()
        {
            var retorno = _calendarioService.ListarMesesAsync();

            return Ok(retorno);
        }
    }
}
 