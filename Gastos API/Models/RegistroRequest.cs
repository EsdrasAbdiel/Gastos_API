namespace Gastos_API.Models
{
    public class RegistroRequest
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public int Senha { get; set; }
        public int ConfirmarSenha { get; set; }
    }
}
