using System.Security.Claims;
using DreamApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DreamApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SeguidorController : ControllerBase
    {
        private SeguidorService seguidorService;

        public SeguidorController(SeguidorService seguidorService)
        {
            this.seguidorService = seguidorService;
        }

        // o id vem do token (quem está logado), não da URL
        private int? IdDoToken()
        {
            string? texto = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return int.TryParse(texto, out int id) ? id : null;
        }

        [HttpGet("Seguindo")]
        public IActionResult Seguindo()
        {
            int? id = IdDoToken();

            if (id == null)
            {
                return Unauthorized();
            }

            return Ok(seguidorService.ListarSeguidos(id.Value));
        }

        [HttpGet("Segue")]
        public IActionResult Segue([FromQuery] int idAlvo)
        {
            int? id = IdDoToken();

            if (id == null)
            {
                return Unauthorized();
            }

            return Ok(new { segue = seguidorService.Segue(id.Value, idAlvo) });
        }

        [HttpPost("Seguir")]
        public IActionResult Seguir([FromQuery] int idAlvo)
        {
            int? id = IdDoToken();

            if (id == null)
            {
                return Unauthorized();
            }

            // mensagens específicas para descobrir o motivo da recusa
            if (id.Value == idAlvo)
            {
                return BadRequest($"Você não pode seguir a si mesmo. Id do token: {id.Value}, idAlvo: {idAlvo}.");
            }

            if (!seguidorService.UsuarioExiste(idAlvo))
            {
                return BadRequest($"O usuário {idAlvo} não existe no banco da API. Id do token: {id.Value}.");
            }

            seguidorService.Seguir(id.Value, idAlvo);

            return Ok();
        }

        [HttpDelete("Deixar")]
        public IActionResult Deixar([FromQuery] int idAlvo)
        {
            int? id = IdDoToken();

            if (id == null)
            {
                return Unauthorized();
            }

            seguidorService.DeixarDeSeguir(id.Value, idAlvo);

            return Ok();
        }
    }
}