using Gastos_API.Data;
using Gastos_API.DTOs;
using Gastos_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gastos_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DespesaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DespesaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Listar()
        {
            var despesas = await _context.Despesa.Include(d => d.Categoria).Select(d => new DespesaDTO
            {
                Id = d.Id,
                Descricao = d.Descricao,
                CategoriaId = d.CategoriaId,
                Categoria = new CategoriaDTO
                {
                    Id = d.Categoria.Id,
                    Descricao = d.Categoria.Descricao
                }
            }).ToListAsync();

            return Ok(despesas);
        }

        [HttpPost]
        public async Task<ActionResult> Criar(DespesaRequest request)
        {
            var despesa = new Despesa
            {
                Descricao = request.Descricao,
                CategoriaId = request.CategoriaId,
            };

            _context.Despesa.Add(despesa);
            await _context.SaveChangesAsync();

            return Ok(new { sucesso = true, mensagem = "Despesa cadastrada com sucesso"});
        }
    }
}
