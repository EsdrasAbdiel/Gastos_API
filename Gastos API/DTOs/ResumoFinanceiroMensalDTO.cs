namespace Gastos_API.DTOs
{
    public class EntradaDTO
    {
        public int Id { get; set; }
        public string EntradaDescricao { get; set; }
        public decimal EntradaValor { get; set; }
        public Guid Entrada_Id { get; set; }
    }
}
