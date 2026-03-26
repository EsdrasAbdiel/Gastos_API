namespace Gastos_API.Models
{
    public class EntradaItem
    {
        public int Id { get; set; }
        public string EntradaDescricao { get; set; }
        public decimal EntradaValor { get; set; }
        public Guid Entrada_Id { get; set; }
        public ResumoFinanceiroMensal? Despesa { get; set; }
    }
}
