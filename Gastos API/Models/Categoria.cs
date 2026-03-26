using System.ComponentModel.DataAnnotations.Schema;

namespace Gastos_API.Models
{
    public class Categoria
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("descricao")]
        public string Descricao { get; set; }
        public ICollection<Despesa> Despesas { get; set; }
    }
}
