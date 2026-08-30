using System.Text.Json.Serialization;

namespace Gastos_API.Models
{
    public class ImportacaoExtratoRequest
    {
        [JsonPropertyName("usuarioId")]
        public Guid UsuarioId { get; set; }

        [JsonPropertyName("extrato")]
        public List<ExtratoItem> Extrato { get; set; } = new();
    }
}
