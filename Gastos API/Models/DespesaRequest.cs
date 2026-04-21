using Gastos_API.DTOs;

namespace Gastos_API.Models
{
    public class DespesaRequest
    {
        public string Descricao { get; set; }
        public int CategoriaId { get; set; }
    }
}
