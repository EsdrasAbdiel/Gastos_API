namespace Gastos_API.Models
{
    public class FluxoDeCaixaRequest
    {
        public Guid Id { get; set; }
        public decimal? ValorTotal { get; set; }
        public DateTime DataInclusao { get; set; }
        public int Ano { get; set; }
        public int Mes { get; set; }
        public required List<DespesaItem> Despesas { get; set; }
        public required List<EntradaItem> Entradas { get; set; }
    }
}
