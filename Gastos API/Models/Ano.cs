using Gastos_API.Enums;

namespace Gastos_API.Models
{
    public class Ano
    {
        public int Id { get; set; }
        public int AnoDescricao { get; set; }
        public StatusCompetencia StatusCompetenciaAno { get; set; }
    }
}
