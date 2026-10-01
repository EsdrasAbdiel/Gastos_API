using Gastos_API.Models;
using Gastos_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gastos_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController(IDashboardService dashboardService) : ControllerBase
    {
        private readonly IDashboardService _dashboardService = dashboardService;

        [HttpGet]
        public async Task<ActionResult<Dashboard>> Get(Guid id)
        {
            return Ok(await _dashboardService.ListarAsync(id));
        }

    }
}
