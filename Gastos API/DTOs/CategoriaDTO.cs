using System.ComponentModel.DataAnnotations.Schema;

namespace Gastos_API.DTOs
{
    public class CategoriaDTO
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
    }
}
