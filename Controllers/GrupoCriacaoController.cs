using System.Security.Claims;
using DreamApi.Models;
using DreamApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DreamApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GrupoCriacaoController : ControllerBase
    {
        private GrupoCriacaoService grupoCriacaoService;

        public GrupoCriacaoController(GrupoCriacaoService grupoCriacaoService)
        {
            this.grupoCriacaoService = grupoCriacaoService;
        }

        // o id vem do token (quem está logado), não da URL ou do corpo
        private int? IdDoToken()
        {
            string? texto = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return int.TryParse(texto, out int id) ? id : null;
        }

        [HttpPost("Criar")]
        public IActionResult Criar([FromBody] CriarGrupoDto dto)
        {
            int? idUsuario = IdDoToken();

            if (idUsuario == null)
            {
                return Unauthorized();
            }

            if (string.IsNullOrWhiteSpace(dto.Nome))
            {
                return BadRequest("Informe o nome do grupo.");
            }

            dto.IdUsuario = idUsuario.Value;

            int id = grupoCriacaoService.Criar(dto);

            return Ok(new { id });
        }

        [HttpGet("{id:int}")]
        public IActionResult Obter(int id)
        {
            int? idUsuario = IdDoToken();

            if (idUsuario == null)
            {
                return Unauthorized();
            }

            GrupoDetalheDto? grupo = grupoCriacaoService.Obter(id, idUsuario.Value);

            return grupo == null ? NotFound() : Ok(grupo);
        }

        [HttpPost("{id:int}/Seguir")]
        public IActionResult Seguir(int id)
        {
            int? idUsuario = IdDoToken();

            if (idUsuario == null)
            {
                return Unauthorized();
            }

            grupoCriacaoService.Seguir(id, idUsuario.Value);

            return Ok();
        }

        [HttpDelete("{id:int}/Seguir")]
        public IActionResult Deixar(int id)
        {
            int? idUsuario = IdDoToken();

            if (idUsuario == null)
            {
                return Unauthorized();
            }

            grupoCriacaoService.Deixar(id, idUsuario.Value);

            return Ok();
        }
    }
}