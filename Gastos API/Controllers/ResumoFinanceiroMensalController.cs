using Gastos_API.Data;
using Gastos_API.Models;
using Gastos_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Gastos_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ResumoFinanceiroMensalController : ControllerBase
    {
        private readonly ICompetenciaService _competenciaService;
        private readonly IDespesaService _despesaService;
        private readonly IEntradaService _entradaService;
        private readonly IResumoFinanceiroMensalService _resumoFinanceiroMensalService;

        public ResumoFinanceiroMensalController(
            ICompetenciaService competenciaService,
            IDespesaService despesaService,
            IEntradaService entradaService,
            IResumoFinanceiroMensalService resumoFinanceiroMensalService
            )
        {
            _competenciaService = competenciaService;
            _despesaService = despesaService;
            _entradaService = entradaService;
            _resumoFinanceiroMensalService = resumoFinanceiroMensalService;
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarResumoFinanceiroAsync([FromBody] ResumoFinanceiroMensalRequest request)
        {
            try
            {
                if (request.Id == Guid.Empty)
                    request.Id = Guid.NewGuid();

                await _resumoFinanceiroMensalService.CadastrarResumoFinanceiroAsync(request);

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

        [HttpGet("{id}")]
        public async Task<ActionResult> BuscarResumoFinanceiroPorIdAsync(Guid id)
        {

            var resumoFinanceiroMensal = await _resumoFinanceiroMensalService.BuscarResumoFinanceiroPorIdAsync(id);
            return Ok(resumoFinanceiroMensal);
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarDespesa([FromBody] ResumoFinanceiroMensalRequest request)
        {
            var data = DateTime.Now;

            var despesa = await _resumoFinanceiroMensalService.BuscarDespesaComItensPorIdAsync(request.Id);

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
                despesa.StatusCompetenciaMes = _competenciaService.VerificarStatusCompetenciaPeriodo(data.Month, request.Mes);

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
                            entradaExistente.DataPagamento = itemEntradaReq.DataPagamento;
                        }
                    }
                    else
                    {
                        var novoItem = new EntradaItem
                        {
                            Entrada_Id = despesa.Id,
                            EntradaDescricao = itemEntradaReq.EntradaDescricao,
                            EntradaValor = itemEntradaReq.EntradaValor,
                            DataPagamento = itemEntradaReq.DataPagamento
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarPeloId(Guid id)
        {
            await _resumoFinanceiroMensalService.RemoverDespesaAsync(id);

            return Ok(new { sucesso = true, message = "Resumo financeiro excluido com sucesso" });
        }
    }
}
