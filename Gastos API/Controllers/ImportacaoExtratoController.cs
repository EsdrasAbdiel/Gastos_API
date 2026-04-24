using Microsoft.AspNetCore.Mvc;

namespace Gastos_API.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class ImportacaoExtratoController : Controller
    {
        [HttpPost("importar")]
        public async Task<IActionResult> Importar(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Arquivo inválido");

            using var content = new MultipartFormDataContent();
            content.Add(new StreamContent(file.OpenReadStream()), "file", file.FileName);

            var client = new HttpClient();
            var response = await client.PostAsync("http://localhost:8000/extrair", content);

            var json = await response.Content.ReadAsStringAsync();

            return Ok(json);
        }
    }
}
