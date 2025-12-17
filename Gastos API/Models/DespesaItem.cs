using System.ComponentModel.DataAnnotations.Schema;

namespace Gastos_API.Models
{
    public class DespesaItem
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("descricao")]
        public string Descricao { get; set; }
        [Column("valor")]
        public decimal Valor { get; set; }
        [Column("despesaid")]
        public Guid DespesaId { get; set; }
    }
}
