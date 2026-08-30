 using Gastos_API.Data;
using Gastos_API.Models;
using Gastos_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Gastos_API.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class ImportacaoExtratoController : ControllerBase
    {
        private readonly IResumoFinanceiroMensalService _resumoFinanceiroMensalService;
        private readonly IDespesaService _despesaService;
        private readonly IEntradaService _entradaService;
        private readonly ICalendarioService _calendarioService;

        private readonly AppDbContext _context;

        public ImportacaoExtratoController(
            IResumoFinanceiroMensalService resumoFinanceiroMensalService,
            IDespesaService despesaService,
            IEntradaService entradaService,
            ICalendarioService calendarioService,
            AppDbContext context)
        {
            _resumoFinanceiroMensalService = resumoFinanceiroMensalService;
            _despesaService = despesaService;
            _entradaService = entradaService;
            _calendarioService = calendarioService;
            _context = context;

        }

        [HttpPost("importar")]
        public async Task<IActionResult> Importar(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new
                {
                    sucesso = false,
                    mensagem = "Arquivo inválido."
                });

            try
            {
                using var client = new HttpClient
                {
                    Timeout = TimeSpan.FromMinutes(5)
                };

                using var content = new MultipartFormDataContent();
                content.Add(new StreamContent(file.OpenReadStream()), "file", file.FileName);

                var response = await client.PostAsync("http://localhost:8000/extrair", content);

                if (!response.IsSuccessStatusCode)
                {
                    var erro = await response.Content.ReadAsStringAsync();

                    return StatusCode((int)response.StatusCode, new
                    {
                        sucesso = false,
                        mensagem = "Erro ao processar o PDF na API Python.",
                        detalhe = erro
                    });
                }

                var json = await response.Content.ReadAsStringAsync();

                var dados = JsonSerializer.Deserialize<List<ExtratoItem>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                Console.WriteLine(JsonSerializer.Serialize(dados, new JsonSerializerOptions
                {
                    WriteIndented = true
                }));


                if (dados == null || !dados.Any())
                {
                    return Ok(new
                    {
                        sucesso = true,
                        mensagem = "Nenhum registro encontrado no PDF.",
                        resultado = new List<ExtratoItem>()
                    });
                }

                var resultado = dados
                    .GroupBy(x => new
                    {
                        x.Descricao,
                        x.Data,
                        x.Tipo
                    })
                    .Select(g => new ExtratoItem
                    {
                        Descricao = g.Key.Descricao,
                        Data = g.Key.Data,
                        Tipo = g.Key.Tipo,
                        Valor = g.Sum(x => x.Valor)
                    })
                    .ToList();

                Console.WriteLine(resultado);

                return Ok(new
                {
                    sucesso = true,
                    mensagem = "Extrato importado com sucesso.",
                    resultado
                });
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    sucesso = false,
                    mensagem = "Não foi possível conectar à API Python.",
                    erro = ex.Message
                });
            }
        }

        [HttpPost("cadastrarResumoFinanceiro/importacaoExtrato")]
        public async Task<IActionResult> CadastrarPelaImportacaoDeExtrato([FromBody] ImportacaoExtratoRequest request)
        {
            if (request?.Extrato == null || !request.Extrato.Any())
                return BadRequest(new
                {
                    sucesso = false,
                    mensagem = "Nenhum lançamento informado."
                });

            if (request.UsuarioId == Guid.Empty)
                return BadRequest(new
                {
                    sucesso = false,
                    mensagem = "Usuário inválido."
                });

            var extrato = request.Extrato;

            var datasComValor = extrato
                .Select(x => x.Data)
                .ToList();

            if (!datasComValor.Any())
            {
                return BadRequest(new
                {
                    sucesso = false,
                    mensagem = "Informe a data dos lançamentos."
                });
            }

            var menorData = datasComValor.Min();
            var maiorData = datasComValor.Max();

            var ano = menorData.Year;
            var mes = menorData.Month;

            var dataAtual = DateOnly.FromDateTime(DateTime.Now);

            var entradasExtrato = extrato
                .Where(x => x.Valor > 0)
                .Select(x => new EntradaItem
                {
                    EntradaDescricao = x.Descricao,
                    EntradaValor = x.Valor,
                    DataPagamento = x.Data 
                })
                .ToList();

            var despesasExtrato = extrato
                .Where(x => x.Valor < 0)
                .Select(x => new DespesaItem
                {
                    Descricao = x.Descricao,
                    Valor = Math.Abs(x.Valor),
                    Pago = true,
                    DataInclusao = x.Data
                })
                .ToList();

            var valorEntradaImportado = entradasExtrato.Sum(x => x.EntradaValor);
            var valorDespesaImportado = despesasExtrato.Sum(x => x.Valor);



            try
            {
                var resumoExistente = await _resumoFinanceiroMensalService.BuscarPorAnoEMes(ano, mes, request.UsuarioId);

                if (resumoExistente == null)
                {
                    var resumoId = Guid.NewGuid();

                    var novoResumo = new ResumoFinanceiroMensal
                    {
                        Id = resumoId,
                        UsuarioId = request.UsuarioId,
                        Ano = ano,
                        Mes = mes,
                        DataInclusao = dataAtual,
                        ValorEntradaTotal = valorEntradaImportado,
                        ValorDespesaTotal = valorDespesaImportado,
                        StatusCompetenciaMes = _calendarioService.VerificarStatusCompetenciaPeriodo(dataAtual.Month, mes)
                    };

                    await _resumoFinanceiroMensalService.AdicionarDespesaAsync(novoResumo);

                    foreach (var entrada in entradasExtrato)
                    {
                        entrada.Entrada_Id = resumoId;
                        await _entradaService.AdicionarNovaEntradaItemAsync(entrada);
                    }

                    foreach (var despesa in despesasExtrato)
                    {
                        despesa.DespesaId = resumoId;
                        await _despesaService.AdicionarNovaDespesaItemAsync(despesa);
                    }

                    var importacaoExtrato = new ImportacaoExtrato
                    {
                        UsuarioId = request.UsuarioId,
                        DataImportacao = dataAtual,
                        Status = Enums.StatusImportacaoExtrato.Concluido,
                        QuantidadeLancamentos = 1,
                        ReferenciaMes = mes,
                        IdResumoFinanceiro = resumoId
                    };

                    _context.ImportacaoExtrato.Add(importacaoExtrato);
                    await _context.SaveChangesAsync();

                    return Ok(new
                    {
                        sucesso = true,
                        mensagem = "Importação de extrato efetuado com sucesso.",
                    });
                }

                resumoExistente.ValorEntradaTotal =
                    (resumoExistente.ValorEntradaTotal ?? 0) + valorEntradaImportado;

                resumoExistente.ValorDespesaTotal =
                    (resumoExistente.ValorDespesaTotal ?? 0) + valorDespesaImportado;

                resumoExistente.DataInclusao =
                    resumoExistente.DataInclusao;

                resumoExistente.StatusCompetenciaMes =
                    _calendarioService.VerificarStatusCompetenciaPeriodo(dataAtual.Month, mes);

                foreach (var entrada in entradasExtrato)
                {
                    entrada.Entrada_Id = resumoExistente.Id;
                    await _entradaService.AdicionarNovaEntradaItemAsync(entrada);
                }

                foreach (var despesa in despesasExtrato)
                {
                    despesa.DespesaId = resumoExistente.Id;
                    await _despesaService.AdicionarNovaDespesaItemAsync(despesa);
                }

                await _resumoFinanceiroMensalService.AtualizarDespesaAsync(resumoExistente);

                var importacaoExtratoAtualizado = new ImportacaoExtrato
                {
                    UsuarioId = request.UsuarioId,
                    DataImportacao = dataAtual,
                    Status = Enums.StatusImportacaoExtrato.Concluido,
                    QuantidadeLancamentos = 1,
                    ReferenciaMes = mes,
                    IdResumoFinanceiro = resumoExistente.Id
                };

                _context.ImportacaoExtrato.Add(importacaoExtratoAtualizado);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    sucesso = true,
                    mensagem = "Resumo financeiro atualizado com sucesso.",
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return StatusCode(500, new
                {
                    sucesso = false,
                    mensagem = "Erro ao salvar importação do extrato.",
                    erro = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        [HttpGet("{usuarioId}")]
        public async Task<IActionResult> BuscarExtratosAsync(Guid usuarioId)
        {
            if (usuarioId == Guid.Empty) { return BadRequest(); }

            var extratos = await _context.ImportacaoExtrato
    .Where(x => x.UsuarioId == usuarioId)
    .ToListAsync();
            return Ok(extratos);
        }

    }

}


