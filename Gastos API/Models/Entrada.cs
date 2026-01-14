namespace Gastos_API.Models
{
    public class Entrada
    {
        public Guid Id { get; set; }
        public Guid Despesa_Id { get; set; }
        public decimal? ValorTotal { get; set; }
        public List<EntradaItem> EntradaItens { get; set; }

    }
}
