using System.ComponentModel.DataAnnotations.Schema;

namespace Gastos_API.Models
{
    public class Registro
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("nome")]
        public string Nome { get; set; } = string.Empty;
        [Column("email")]
        public string Email { get; set; } = string.Empty;
        [Column("datanascimento")]
        public DateTime DataNascimento { get; set; }
        [Column("senha")]
        public int Senha { get; set; }
        [Column("confirmarsenha")]
        public int ConfirmarSenha { get; set; }
    }
}
