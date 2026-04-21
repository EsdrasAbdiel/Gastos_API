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

            var emailExistente = await _authService.BuscarUsuarioPeloEmailAsync(registroRequest.Email);

            if (emailExistente != null)
                return BadRequest(new
                {
                    mensagem = "Email já está sendo utlizado por outro usuário",
                    sucesso = false,
                });

            var response = await _authService.AdicionarRegistroAsync(registro);

            return Ok(new 
            { 
                mensagem = "Cadastro efetuado com sucesso",
                sucesso = true,
                resultado = response
            });
        }

        [HttpPost("buscarUsuario/")]
        public async Task<ActionResult> BuscarUsuarioPeloEmail([FromBody] Registro registro)
        {
            var response = await _authService.BuscarUsuarioPeloEmailAsync(registro.Email);
            Console.WriteLine(response);

            if (response == null)
            {
                return BadRequest(new
                {
                    mensagem = "Email ou senha incorretos",
                    sucesso = false
                });
            }

            if (response.Email != registro.Email)
                return BadRequest(new
                {
                    mensagem = "Email incorreto",
                    sucesso = false
                });
            
            if(response.Senha != registro.Senha)
                return BadRequest(new
                {
                    mensagem = "Senha incorreta",
                    sucesso = false
                });

            return Ok(new
            {
                resultado = response.Id,
                mensagem = "Login efetuado com sucesso",
                sucesso = true
            });
        }
    }
}
