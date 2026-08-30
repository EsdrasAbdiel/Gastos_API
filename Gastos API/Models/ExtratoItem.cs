using System.Text.Json.Serialization;

namespace Gastos_API.Models
{
    public class ExtratoItem
    {
        [JsonPropertyName("data")]
        public DateOnly Data { get; set; }

        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        [JsonPropertyName("valor")]
        public decimal Valor { get; set; }

        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }
    }
}
