using Gastos_API.Data;
using Gastos_API.Models;
using Microsoft.EntityFrameworkCore;
using Gastos_API.Services;

namespace Gastos_API.Interfaces
{
    public class ResumoFinanceiroMensalRepository : IResumoFinanceiroMensalService
    {
        private readonly AppDbContext _context;
        private readonly IDespesaService _despesaService;
        private readonly IEntradaService _entradaService;
        private readonly ICompetenciaService _competenciaService;

        public ResumoFinanceiroMensalRepository(
            AppDbContext context,
            IDespesaService despesaService,
            IEntradaService entradaService,
            ICompetenciaService competenciaService
            )
        {
            _context = context;
            _despesaService = despesaService;
            _entradaService = entradaService;
            _competenciaService = competenciaService;
        }

        public async Task<IEnumerable<ResumoFinanceiroMensal>> ListarResumosFinanceiroAsync(int ano)
        {
            return await _context.ResumoFinanceiroMensal
                                 .Where(d => d.Ano == ano)
                                 .Select(d => new ResumoFinanceiroMensal
                                 {
                                     Id = d.Id,
                                     Ano = d.Ano,
                                     Mes = d.Mes
                                 })
                                 .ToListAsync();
        }

        public async Task<ResumoFinanceiroMensal?> BuscarPorAnoEMes(int ano, int mes, Guid usuarioId)
        {
            return await _context.ResumoFinanceiroMensal
                .FirstOrDefaultAsync(x => x.Ano == ano && x.Mes == mes && x.UsuarioId == usuarioId);
        }

        public async Task<List<ResumoFinanceiroMensal>> BuscarResumoFinanceiroPeloAno(int ano, Guid usuarioId)
        {
            return await _context.ResumoFinanceiroMensal.Where(d => d.UsuarioId == usuarioId && d.Ano == ano).ToListAsync();
        }

        public async Task<ResumoFinanceiroMensal?> BuscarResumoFinanceiroPorIdAsync(Guid id)
        {
            var data = DateTime.Now;

            var despesa = await _context.ResumoFinanceiroMensal.Include(x => x.Usuario).FirstOrDefaultAsync(d => d.Id == id) ?? throw new Exception("Despesa não encontrada.");
            
            var itensDespesas = await _despesaService.BuscarItensDespesaPorIdAsync(id);

            var itensEntradas = await _entradaService.BuscarItensEntradaPorIdAsync(id);

            var resumoFinanceiroMensal = new ResumoFinanceiroMensal
            {
                Id = despesa.Id,
                UsuarioId = despesa.UsuarioId,
                ValorDespesaTotal = despesa.ValorDespesaTotal,
                ValorEntradaTotal = despesa.ValorEntradaTotal,
                DataInclusao = despesa.DataInclusao,
                Mes = despesa.Mes,
                Ano = despesa.Ano,
                ItensDespesa = itensDespesas,
                ItensEntrada = itensEntradas,
                StatusCompetenciaMes = _competenciaService.VerificarStatusCompetenciaPeriodo(data.Month, despesa.Mes)
            };

            return resumoFinanceiroMensal;
        }

        public async Task<ResumoFinanceiroMensal?> BuscarDespesaComItensPorIdAsync(Guid id)
        {
            return await _context.ResumoFinanceiroMensal
                                 .Include(d => d.ItensDespesa)
                                 .Include(d => d.ItensEntrada)
                                 .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<ResumoFinanceiroMensal?> BuscarEntradaComItensPorIdAsync(Guid id)
        {
            return await _context.ResumoFinanceiroMensal
                                 .Include(d => d.ItensEntrada)
                                 .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<ResumoFinanceiroMensal> CadastrarResumoFinanceiroAsync(ResumoFinanceiroMensalRequest resumoFinanceiroMensal)
        {
            var retorno = ResumoFinanceiroMensalAsync(resumoFinanceiroMensal);

            _context.ResumoFinanceiroMensal.Add(retorno);
            await _context.SaveChangesAsync();
            return retorno;
        }

        public async Task AtualizarDespesaAsync(ResumoFinanceiroMensal despesa)
        {
            _context.ResumoFinanceiroMensal.Update(despesa);
            await _context.SaveChangesAsync();
        }

        public void RemoverDespesaAsync(ResumoFinanceiroMensal despesa)
        {
            _context.ResumoFinanceiroMensal.Remove(despesa);
        }

        public async Task SalvarChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeletarDespesaAsync(Guid id)
        {
            var despesa = await _context.ResumoFinanceiroMensal.FindAsync(id);
            if (despesa != null)
            {
                _context.ResumoFinanceiroMensal.Remove(despesa);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ResumoFinanceiroMensal>> ListarResumoFinanceiroPorUsuarioId(Guid id)
        {
            var resumo = await _context.ResumoFinanceiroMensal
                .Where(x => x.UsuarioId == id)
                .ToListAsync();

            return resumo;
        }

        public async Task<ResumoFinanceiroMensal> CadastrarResumoFinanceiroImportacaoAsync(ResumoFinanceiroMensal resumoFinanceiroMensal)
        {
            _context.ResumoFinanceiroMensal.Add(resumoFinanceiroMensal);
            await _context.SaveChangesAsync();
            return resumoFinanceiroMensal;
        }

        public async Task RemoverDespesaAsync(Guid id)
        {
            var resumo = await _context.ResumoFinanceiroMensal
                .FirstOrDefaultAsync(x => x.Id == id);

            if (resumo == null)
                return;

            _context.ResumoFinanceiroMensal.Remove(resumo);

            await _context.SaveChangesAsync();
        }

        private ResumoFinanceiroMensal ResumoFinanceiroMensalAsync(ResumoFinanceiroMensalRequest resumoFinanceiroMensal)
        {
            var data = DateTime.Now;

            return new ResumoFinanceiroMensal
            {
                Id = resumoFinanceiroMensal.Id,
                ValorDespesaTotal = resumoFinanceiroMensal.ValorDespesaTotal,
                ValorEntradaTotal = resumoFinanceiroMensal.ValorEntradaTotal,
                ItensDespesa = resumoFinanceiroMensal.Despesas,
                ItensEntrada = resumoFinanceiroMensal.Entradas,
                DataInclusao = resumoFinanceiroMensal.DataInclusao,
                Mes = resumoFinanceiroMensal.Mes,
                Ano = resumoFinanceiroMensal.Ano,
                UsuarioId = resumoFinanceiroMensal.UsuarioId,
                StatusCompetenciaMes = _competenciaService.VerificarStatusCompetenciaPeriodo(data.Month, resumoFinanceiroMensal.Mes)
            };
        }
    }
}
