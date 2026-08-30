using Gastos_API.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gastos_API.Models
{
    public class ResumoFinanceiroMensal
    {
        public StatusCompetencia StatusCompetenciaMes { get; set; }
        [Column("usuarioId")]
        public Guid UsuarioId { get; set; }
        [Column("id")]
        public Guid Id { get; set; }
        [Column("valordespesatotal")]
        public decimal? ValorDespesaTotal { get; set; }
        [Column("valorentradatotal")]
        public decimal? ValorEntradaTotal { get; set; }
        [Column("datainclusao")]
        public DateOnly DataInclusao { get; set; }
        [Column("mes")]
        public int Mes { get; set; }
        [Column("ano")]
        public int Ano { get; set; }
        [NotMapped]
        public List<DespesaItem> ItensDespesa { get; set; } = new();
        [NotMapped]
        public List<EntradaItem> ItensEntrada { get; set; } = new();
        [NotMapped]
        public Registro Usuario { get; set; } = null!;

    }

}
