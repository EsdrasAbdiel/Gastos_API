using Gastos_API.Models;
using Gastos_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gastos_API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registro([FromBody] RegistroRequest registroRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { mensagem = "Erro ao efetuar o cadastro", sucesso = false });

            var registro = new Registro
            {
                Id = Guid.NewGuid(),
                DataNascimento = registroRequest.DataNascimento,
                Nome = registroRequest.Nome,
                Email = registroRequest.Email,
                Senha = registroRequest.Senha,
                ConfirmarSenha = registroRequest.ConfirmarSenha
            };

            var response = await _authService.AdicionarRegistroAsync(registro);

            return Ok(new 
            { 
                mensagem = "Cadastro efetuado com sucesso",
                sucesso = true,
                resultado = response
            });
        }
    }
}
