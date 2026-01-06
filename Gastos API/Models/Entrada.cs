namespace Gastos_API.Models
{
    public class Entrada
    {
        public Guid Id { get; set; }
        public decimal? ValorTotal { get; set; }
        public List<EntradaItem> EntradaItens { get; set; }
        public Guid DespesaId { get; set; }

    }
}
