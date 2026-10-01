using Gastos_API.Models;
using Gastos_API.Services;
using System.Text.Json;

namespace Gastos_API.Repositories
{
    public class ImportacaoExtratoRespository : IImportacaoExtratoService
    {
        public async Task<HttpResponseMessage> ConexaoServicoDeImportacaoAsync(IFormFile file)
        {
            using var client = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(5)
            };

            using var content = new MultipartFormDataContent();
            content.Add(new StreamContent(file.OpenReadStream()), "file", file.FileName);

            var response = await client.PostAsync("http://localhost:8000/extrair", content);

            return response;
        }

        public async Task<List<ExtratoItem>?> CarregarDadosExtraidosDoPdfAsync(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<ExtratoItem>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
