namespace Gastos_API.Models
{
    public class Dashboard
    {
        public decimal TotalDespesas { get; set; }
        public decimal TotalEntradas { get; set; }
        public decimal TotalSaldo { get; set; }
        public int QuantidadeRegistro { get; set; }
        public List<ResumoFinanceiroMensal>? Registros { get; set; }
    }
}
