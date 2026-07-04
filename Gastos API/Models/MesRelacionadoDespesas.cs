using Gastos_API.Enums;

namespace Gastos_API.Models
{
    public class MesRelacionadoDespesas
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string NomeAbreviado { get; set; }
        public Guid? DespesaId { get; set; }
        public StatusCompetencia StatusCompetenciaMes { get; set; }
        public int ValorSaidaTotal { get; set; }
    }
}
