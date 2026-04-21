using Gastos_API.Data;
using Gastos_API.Models;
using Gastos_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gastos_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ResumoFinanceiroMensalController : ControllerBase
    {
        private readonly IDespesaService _despesaService;
        private readonly IEntradaService _entradaService;
        private readonly IResumoFinanceiroMensalService _resumoFinanceiroMensalService;
        private readonly IConfiguration Configuration;

        public ResumoFinanceiroMensalController(
            IDespesaService despesaService, 
            IEntradaService entradaService, 
            IResumoFinanceiroMensalService resumoFinanceiroMensalService,
            IConfiguration configuration
            )
        {
            _despesaService = despesaService;
            _entradaService = entradaService;
            _resumoFinanceiroMensalService = resumoFinanceiroMensalService;
            Configuration = configuration;
        }

        [HttpGet("listar/{ano}")]
        public async Task<ActionResult<IEnumerable<ResumoFinanceiroMensal>>> GetDespesas(int ano)
        {
            var despesas = await _resumoFinanceiroMensalService.BuscarTodasAsDespesasAsync(ano);
            return Ok(despesas);
        }


        [HttpPost("cadastro")]
        public async Task<IActionResult> ReceberDespesas([FromBody] ResumoFinanceiroMensalRequest request)
        {
            try
            {
                if (request.Id == Guid.Empty)
                    request.Id = Guid.NewGuid();

                var despesa = new ResumoFinanceiroMensal
                {
                    Id = request.Id,
                    ValorDespesaTotal = request.ValorDespesaTotal,
                    ValorEntradaTotal = request.ValorEntradaTotal,
                    ItensDespesa = request.Despesas,
                    ItensEntrada = request.Entradas,
                    DataInclusao = request.DataInclusao,
                    Mes = request.Mes,
                    Ano = request.Ano,
                    UsuarioId = request.UsuarioId
                };

                await _resumoFinanceiroMensalService.AdicionarDespesaAsync(despesa);

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

            var despesa = await _resumoFinanceiroMensalService.BuscarDespesaPorIdAsync(id);

            if (despesa == null)
                return NotFound("Despesa não encontrada.");

            var itensDespesas = await _despesaService.BuscarItensDespesaPorIdAsync(id);

            var itensEntradas = await _entradaService.BuscarItensEntradaPorIdAsync(id);

            var despesas = new ResumoFinanceiroMensal
            {
                Id = despesa.Id,
                UsuarioId = despesa.UsuarioId,
                ValorDespesaTotal = despesa.ValorDespesaTotal,
                ValorEntradaTotal = despesa.ValorEntradaTotal,
                DataInclusao = despesa.DataInclusao,
                Mes = despesa.Mes,
                Ano = despesa.Ano,
                ItensDespesa = itensDespesas,
                ItensEntrada = itensEntradas
            };

            return Ok(despesas);
        }

        [HttpPut("atualizarDespesa/{id}")]
        public async Task<IActionResult> AtualizarDespesa(Guid id, [FromBody] ResumoFinanceiroMensalRequest request)
        {
            if (id != request.Id)
                return BadRequest(new { erro = "ID da URL diferente do corpo.", sucesso = false });

            var despesa = await _resumoFinanceiroMensalService.BuscarDespesaComItensPorIdAsync(id);

            if (despesa == null)
                return NotFound(new { erro = "Despesa não encontrada.", sucesso = false });

            try
            {
                // Atualiza campos da despesa
                despesa.ValorDespesaTotal = request.ValorDespesaTotal;
                despesa.ValorEntradaTotal = request.ValorEntradaTotal;
                despesa.DataInclusao = request.DataInclusao;
                despesa.Mes = request.Mes;
                despesa.Ano = request.Ano;

                // IDs que vieram do frontend (exceto 0)
                var idsDespesasDoFrontend = _despesaService.ObterIdsDosItensDespesasExistentes(request.Despesas);

                var idsEntradasDoFrontend = _entradaService.ObterIdsDosItensEntradasExistentes(request.Entradas);

                // 1. Remove itens que não vieram mais
                var itensDespesasParaRemover = _despesaService.ObterItensDespesasParaRemover(despesa.ItensDespesa, idsDespesasDoFrontend);

                var itensEntradasParaRemover = _entradaService.ObterItensEntradasParaRemover(despesa.ItensEntrada, idsEntradasDoFrontend);

                if (itensDespesasParaRemover.Any())
                    _despesaService.RemoverItensDespesaAsync(itensDespesasParaRemover);

                if (itensEntradasParaRemover.Any())
                    _entradaService.RemoverItensEntrada(itensEntradasParaRemover);

                // 2. Atualiza ou insere os itens recebidos
                foreach (var itemReq in request.Despesas)
                {
                    if (itemReq.Id > 0)
                    {
                        // É atualização
                        var itemExistente = _despesaService.ObterItemDespesaExistente(despesa.ItensDespesa, itemReq.Id);

                        if (itemExistente != null)
                        {
                            itemExistente.Descricao = itemReq.Descricao;
                            itemExistente.Valor = itemReq.Valor;
                            itemExistente.Pago = itemReq.Pago;
                        }
                    }
                    else
                    {
                        // É item novo (Id = 0 ou negativo, tanto faz)
                        var novoItem = new DespesaItem
                        {
                            DespesaId = despesa.Id,
                            Descricao = itemReq.Descricao,
                            Valor = itemReq.Valor,
                            Pago = itemReq.Pago
                            // Id é auto-incremento → não precisa setar
                        };

                        await _despesaService.AdicionarNovaDespesaItemAsync(novoItem);
                    }
                }

                foreach (var itemEntradaReq in request.Entradas)
                {
                    if (itemEntradaReq.Id > 0)
                    {
                        // É atualização
                        var entradaExistente = _entradaService.ObterItemEntradaExistente(despesa.ItensEntrada, itemEntradaReq.Id);

                        if (entradaExistente != null)
                        {
                            entradaExistente.EntradaDescricao = itemEntradaReq.EntradaDescricao;
                            entradaExistente.EntradaValor = itemEntradaReq.EntradaValor;
                        }
                    }
                    else
                    {
                        var novoItem = new EntradaItem
                        {
                            Entrada_Id = despesa.Id,
                            EntradaDescricao = itemEntradaReq.EntradaDescricao,
                            EntradaValor = itemEntradaReq.EntradaValor
                        };

                        await _entradaService.AdicionarNovaEntradaItemAsync(novoItem);
                    }
                }

                await _resumoFinanceiroMensalService.SalvarChangesAsync();


                return Ok(new { mensagem = "Despesa atualizada com sucesso.", sucesso = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message, sucesso = false });
            }
        }

        [HttpDelete("deletarDespesa/{id}")]
        public async Task<IActionResult> DeletarPeloId(Guid id)
        {
            var despesa = await _resumoFinanceiroMensalService.BuscarDespesaPorIdAsync(id);

            if (despesa == null)
                return NotFound(new
                {
                    message = "Despesa não encontrada.",
                    sucesso = false
                });

            var itensDespesa = await _despesaService.BuscarItensDespesaPorIdAsync(id);

            var itensEntrada = await _entradaService.BuscarItensEntradaPorIdAsync(id);

            _despesaService.RemoverItensDespesaAsync(itensDespesa);

            _entradaService.RemoverItensEntrada(itensEntrada);

            _resumoFinanceiroMensalService.RemoverDespesaAsync(despesa);

            await _resumoFinanceiroMensalService.SalvarChangesAsync();

            return Ok(new
            {
                message = "Despesa excluida com sucesso",
                sucesso = true
            });
        }
    }
}
