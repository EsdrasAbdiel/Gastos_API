namespace Gastos_API.DTOs
{
    public class DespesaDTO
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public int CategoriaId { get; set; }
        public CategoriaDTO Categoria { get; set; }
    }
}
