using System.ComponentModel.DataAnnotations.Schema;

namespace Gastos_API.Models
{
    public class Despesa
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("valortotal")]
        public decimal? ValorTotal { get; set; }
        [Column("datainclusao")]
        public DateTime DataInclusao { get; set; }
        [Column("mes")]
        public int Mes { get; set; }
        [Column("ano")]
        public int Ano { get; set; }
        public List<DespesaItem> ItensDespesa { get; set; }
        public List<EntradaItem> ItensEntrada { get; set; }
    }

}
