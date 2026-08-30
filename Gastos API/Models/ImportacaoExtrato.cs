using Gastos_API.Enums;

namespace Gastos_API.Models
{
    public class ImportacaoExtrato
    {
        public int Id { get; set; }

        public Guid IdResumoFinanceiro { get; set; }

        public Guid UsuarioId { get; set; }

        public DateOnly DataImportacao { get; set; }

        public StatusImportacaoExtrato Status { get; set; }

        public int QuantidadeLancamentos { get; set; }

        public int ReferenciaMes { get; set; }
    }
}
