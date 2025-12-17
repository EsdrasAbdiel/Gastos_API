namespace Gastos_API.Models
{
    public class DespesaRequest
    {
        public Guid Id { get; set; }
        public decimal? ValorTotal { get; set; }
        public DateTime DataInclusao { get; set; }
        public int Ano { get; set; }
        public int Mes { get; set; }
        public required List<DespesaItem> Despesas { get; set; }
    }
}
