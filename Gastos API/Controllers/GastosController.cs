using Microsoft.AspNetCore.Mvc;
using Gastos_API.Models;
using Gastos_API.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Gastos_API.Services;

namespace Gastos_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GastosController : ControllerBase
    {
         private readonly IDespesaService _despesaService;

        public GastosController(IDespesaService despesaService)
        {
            _despesaService = despesaService;
        }

        [HttpGet("listar/{ano}")]
        public async Task<ActionResult<IEnumerable<Despesa>>> GetDespesas(int ano)
        {
            var despesas = await _despesaService.BuscarTodasAsDespesasAsync(ano);
            return Ok(despesas);
        }


        //[HttpPost("cadastro")]
        //public async Task<IActionResult> ReceberDespesas([FromBody] DespesaRequest request)
        //{
        //    if (request.Id == Guid.Empty)
        //        request.Id = Guid.NewGuid();

        //    var despesa = new Despesa
        //    {
        //        Id = request.Id,
        //        ValorTotal = request.ValorTotal,
        //        Itens = request.Despesas
        //    };

        //    _context.Despesas.Add(despesa);
        //    await _context.SaveChangesAsync();

        //    return Ok(new
        //    {
        //        id = despesa.Id,
        //        valorTotal = despesa.ValorTotal,
        //        itens = despesa.Itens
        //    });
        //}

        [HttpPost("cadastro")]
        public async Task<IActionResult> ReceberDespesas([FromBody] FluxoDeCaixaRequest request)
        {
            try
            {
                if (request.Id == Guid.Empty)
                    request.Id = Guid.NewGuid();

                var despesa = new Despesa
                {
                    Id = request.Id,
                    ValorTotal = request.ValorTotal,
                    ItensDespesa = request.Despesas,
                    ItensEntrada = request.Entradas,
                    DataInclusao = request.DataInclusao,
                    Mes = request.Mes,
                    Ano = request.Ano
                };

                await _despesaService.AdicionarDespesaAsync(despesa);

                return Ok(new
                {
                    mensagem = "Cadastro efetuado com sucesso.",
                    sucesso = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    erro = ex.Message,
                    sucesso = false
                });
            }
        }

        [HttpGet("buscarDespesa/{id}")]
        public async Task<ActionResult> BuscarPeloId(Guid id)
        {

            var despesa = await _despesaService.BuscarDespesaPorIdAsync(id);

            if (despesa == null)
                return NotFound("Despesa não encontrada.");

            var itens = await _despesaService.BuscarItensDespesaPorIdAsync(id);

            var despesas = new Despesa
            {
                Id = despesa.Id,
                ValorTotal = despesa.ValorTotal,
                DataInclusao = despesa.DataInclusao,
                Mes = despesa.Mes,
                Ano = despesa.Ano,
                ItensDespesa = itens
            };

            return Ok(despesas);
        }

        //[HttpPut("atualizarDespesa/{id}")]
        //public async Task<IActionResult> AtualizarDespesa(Guid id, [FromBody] FluxoDeCaixaRequest request)
        //{
        //    if (id != request.Id)
        //        return BadRequest(new { erro = "ID da URL diferente do corpo.", sucesso = false });

        //    var despesa = await _context.Despesas
        //        .Include(d => d.ItensDespesa)
        //        .FirstOrDefaultAsync(d => d.Id == id);

        //    if (despesa == null)
        //        return NotFound(new { erro = "Despesa não encontrada.", sucesso = false });

        //    try
        //    {
        //        // Atualiza campos da despesa
        //        despesa.ValorTotal = request.ValorTotal;
        //        despesa.DataInclusao = request.DataInclusao;
        //        despesa.Mes = request.Mes;
        //        despesa.Ano = request.Ano;

        //        // IDs que vieram do frontend (exceto 0)
        //        var idsDoFrontend = request.Despesas
        //            .Where(i => i.Id > 0)
        //            .Select(i => i.Id)
        //            .ToList();

        //        // 1. Remove itens que não vieram mais
        //        var itensParaRemover = despesa.ItensDespesa
        //            .Where(db => !idsDoFrontend.Contains(db.Id))
        //            .ToList();

        //        if (itensParaRemover.Any())
        //            _context.DespesaItens.RemoveRange(itensParaRemover);

        //        // 2. Atualiza ou insere os itens recebidos
        //        foreach (var itemReq in request.Despesas)
        //        {
        //            if (itemReq.Id > 0)
        //            {
        //                // É atualização
        //                var itemExistente = despesa.ItensDespesa.FirstOrDefault(x => x.Id == itemReq.Id);
        //                if (itemExistente != null)
        //                {
        //                    itemExistente.Descricao = itemReq.Descricao;
        //                    itemExistente.Valor = itemReq.Valor;
        //                }
        //            }
        //            else
        //            {
        //                // É item novo (Id = 0 ou negativo, tanto faz)
        //                var novoItem = new DespesaItem
        //                {
        //                    DespesaId = despesa.Id,
        //                    Descricao = itemReq.Descricao,
        //                    Valor = itemReq.Valor
        //                    // Id é auto-incremento → não precisa setar
        //                };
        //                _context.DespesaItens.Add(novoItem);
        //            }
        //        }

        //        await _context.SaveChangesAsync();

        //        return Ok(new { mensagem = "Despesa atualizada com sucesso.", sucesso = true });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { erro = ex.Message, sucesso = false });
        //    }
        //}

        //[HttpDelete("deletarDespesa/{id}")]
        //public async Task<IActionResult> DeletarPeloId(Guid id)
        //{
        //    var despesa = await _context.Despesas
        //        .FirstOrDefaultAsync(d => d.Id == id);

        //    if (despesa == null)
        //        return NotFound(new
        //        {
        //            message = "Despesa não encontrada.",
        //            sucesso = false
        //        });

        //    var itens = await _context.DespesaItens
        //        .Where(i => i.DespesaId == id)
        //        .ToListAsync();

        //    _context.DespesaItens.RemoveRange(itens);

        //    _context.Despesas.Remove(despesa);

        //    await _context.SaveChangesAsync();

        //    return Ok(new
        //    {
        //        message = "Despesa excluida com sucesso",
        //        sucesso = true
        //    });
        //}
    }
}
