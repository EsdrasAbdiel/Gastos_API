using Gastos_API.Models;
using Gastos_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

            var emailExistente = await _authService.BuscarUsuarioPeloEmailAsync(registroRequest);

            if (emailExistente != null)
                return BadRequest(new
                {
                    mensagem = "Email já está sendo utlizado por outro usuário",
                    sucesso = false,
                });

            var response = await _authService.AdicionarRegistroAsync(registroRequest);

            return Ok(new 
            { 
                mensagem = "Cadastro efetuado com sucesso",
                sucesso = true,
                resultado = response
            });
        }

        [HttpPost("buscarUsuario/")]
        public async Task<ActionResult> BuscarUsuarioPeloEmail([FromBody] RegistroRequest registro)
        {
            var response = await _authService.BuscarUsuarioPeloEmailAsync(registro);

            if (response == null)
            {
                return BadRequest(new
                {
                    mensagem = "Email ou senha incorretos",
                    sucesso = false
                });
            }

            var token = _authService.GerarToken(response);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddHours(2)
            };

            Response.Cookies.Append("jwt", token, cookieOptions);
            
            return Ok(new
            {
                resultado = response.Id,
                mensagem = "Login efetuado com sucesso",
                sucesso = true
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return Ok();
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            return Ok();
        }
    }
}
