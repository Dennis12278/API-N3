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

        [HttpPost("Criar")]
        public IActionResult Criar([FromBody] CriarGrupoDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome))
            {
                return BadRequest("Informe o nome do grupo.");
            }

            int id = grupoCriacaoService.Criar(dto);

            return Ok(new { id });
        }

        [HttpGet("{id:int}")]
        public IActionResult Obter(int id, [FromQuery] int idUsuario)
        {
            GrupoDetalheDto? grupo = grupoCriacaoService.Obter(id, idUsuario);

            return grupo == null ? NotFound() : Ok(grupo);
        }

        [HttpPost("{id:int}/Seguir")]
        public IActionResult Seguir(int id, [FromQuery] int idUsuario)
        {
            grupoCriacaoService.Seguir(id, idUsuario);

            return Ok();
        }

        [HttpDelete("{id:int}/Seguir")]
        public IActionResult Deixar(int id, [FromQuery] int idUsuario)
        {
            grupoCriacaoService.Deixar(id, idUsuario);

            return Ok();
        }
    }
}