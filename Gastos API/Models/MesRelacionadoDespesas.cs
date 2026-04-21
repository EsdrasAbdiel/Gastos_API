namespace Gastos_API.Models
{
    public class MesRelacionadoDespesas
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string NomeAbreviado { get; set; }
        public Guid? DespesaId { get; set; }
    }
}
