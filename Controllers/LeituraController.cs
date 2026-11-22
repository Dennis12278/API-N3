using System.Security.Claims;
using DreamApi.Models;
using DreamApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DreamApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LeituraController : ControllerBase
    {
        private ArquivoLeituraService arquivoService;

        public LeituraController(ArquivoLeituraService arquivoService)
        {
            this.arquivoService = arquivoService;
        }

        // o id vem do token (quem está logado), não da URL
        private int? IdDoToken()
        {
            string? texto = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return int.TryParse(texto, out int id) ? id : null;
        }

        // envia (ou troca) o arquivo de leitura da publicação
        [HttpPost("{id:int}/Arquivo")]
        public async Task<IActionResult> Enviar(int id, [FromForm] ArquivoLeituraDto dto)
        {
            int? idUsuario = IdDoToken();

            if (idUsuario == null)
            {
                return Unauthorized();
            }

            string? erro = arquivoService.Validar(dto.Arquivo);

            if (erro != null)
            {
                return BadRequest(erro);
            }

            string? erroEnvio = await arquivoService.Enviar(id, idUsuario.Value, dto.Arquivo!);

            if (erroEnvio != null)
            {
                return BadRequest(erroEnvio);
            }

            return Ok();
        }

        // entrega o arquivo para leitura (precisa de login)
        [HttpGet("{id:int}/Arquivo")]
        public IActionResult Obter(int id)
        {
            var resultado = arquivoService.Obter(id);

            if (resultado == null)
            {
                return NotFound();
            }

            string tipoMime = resultado.Value.tipo == "pdf"
                ? "application/pdf"
                : "text/plain; charset=utf-8";

            return PhysicalFile(resultado.Value.caminho, tipoMime);
        }
    }
}