using System.ComponentModel.DataAnnotations.Schema;

namespace Gastos_API.Models
{
    public class Despesa
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("valordespesatotal")]
        public decimal? ValorDespesaTotal { get; set; }
        [Column("valorentradatotal")]
        public decimal? ValorEntradaTotal { get; set; }
        [Column("datainclusao")]
        public DateTime DataInclusao { get; set; }
        [Column("mes")]
        public int Mes { get; set; }
        [Column("ano")]
        public int Ano { get; set; }
        [NotMapped]
        public List<DespesaItem> ItensDespesa { get; set; } = new();
        [NotMapped]
        public List<EntradaItem> ItensEntrada { get; set; } = new();
    }

}
