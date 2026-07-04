using System.Text.Json.Serialization;

namespace Gastos_API.Models
{
    public class EntradaItem
    {
        public int Id { get; set; }
        public string EntradaDescricao { get; set; }
        public decimal EntradaValor { get; set; }
        public DateTime DataPagamento { get; set; }
        public Guid Entrada_Id { get; set; }
        [JsonIgnore]
        public ResumoFinanceiroMensal? Despesa { get; set; }
    }
}
