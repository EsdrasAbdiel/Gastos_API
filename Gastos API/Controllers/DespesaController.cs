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
    public class DespesaController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IDespesaService _despesaService;

        public DespesaController(
            AppDbContext context, 
            IDespesaService despesaService
            )
        {
            _context = context;
            _despesaService = despesaService;
        }

        [HttpGet]
        public ActionResult<List<Despesa>> ListarAsync()
        {
            var retorno = _despesaService.ListarAsync();

            return Ok(retorno);
        }

        [HttpPost]
        public ActionResult CriarAsync(DespesaRequest request)
        {
            var retorno = _despesaService.CriarAsync(request);

            return Ok(new { sucesso = true, mensagem = "Despesa cadastrada com sucesso", resultado = retorno });
        }
    }
}
