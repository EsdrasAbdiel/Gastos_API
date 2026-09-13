using Gastos_API.Data
using Gastos_API.Services;
using Microsoft.AspNetCore.Mvc;

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
    }
}
