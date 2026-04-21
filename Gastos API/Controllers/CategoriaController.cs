using Gastos_API.Data;
using Gastos_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gastos_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> BuscarCategoriasAsync()
        {
            var categorias = _context.Categoria.ToList();

            return Ok(categorias);
        }
    }
}
