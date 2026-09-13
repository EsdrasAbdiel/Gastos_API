using Gastos_API.Models;

namespace Gastos_API.Services
{
    public interface IImportacaoExtratoService
    {
        Task<HttpResponseMessage> ConexaoServicoDeImportacaoAsync(IFormFile file);
        Task<List<ExtratoItem>?> CarregarDadosExtraidosDoPdfAsync(HttpResponseMessage response);
    }
}
