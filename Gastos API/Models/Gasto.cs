using System.ComponentModel.DataAnnotations.Schema;

namespace Gastos_API.Models
{
    public class Gasto
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("descricao")]
        public string Descricao { get; set; } = string.Empty;
        [Column("valor")]
        public decimal Valor { get; set; }
        [Column("data_inclusao")]
        public DateTime Data_Inclusao { get; set; }

    }
}
