namespace Gastos_API.Models
{
    public class RegistroRequest
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public DateTime DataNascimento { get; set; }
        public int Sexo { get; set; }
        public string? Descricao { get; set; }
        public string Senha { get; set; }
        public string ConfirmacaoSenha { get; set; }
    }
}
