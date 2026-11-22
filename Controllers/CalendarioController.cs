using System.Security.Claims;
using DreamApi.Models;
using DreamApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DreamApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalendarioController : ControllerBase
    {
        private CalendarioService calendarioService;

        public CalendarioController(CalendarioService calendarioService)
        {
            this.calendarioService = calendarioService;
        }

        // o id vem do token (quem está logado), não da URL ou do corpo
        private int? IdDoToken()
        {
            string? texto = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return int.TryParse(texto, out int id) ? id : null;
        }

        [HttpGet]
        public IActionResult Listar()
        {
            int? id = IdDoToken();

            if (id == null)
            {
                return Unauthorized();
            }

            return Ok(calendarioService.Listar(id.Value));
        }

        [HttpPost]
        public IActionResult Adicionar([FromBody] Calendario calendario)
        {
            int? id = IdDoToken();

            if (id == null)
            {
                return Unauthorized();
            }

            calendario.IdUsuario = id.Value;

            return Ok(calendarioService.Adicionar(calendario));
        }

        [HttpPut]
        public IActionResult Editar([FromBody] Calendario calendario)
        {
            int? id = IdDoToken();

            if (id == null)
            {
                return Unauthorized();
            }

            calendario.IdUsuario = id.Value;

            return calendarioService.Editar(calendario) ? Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Excluir(int id)
        {
            int? idUsuario = IdDoToken();

            if (idUsuario == null)
            {
                return Unauthorized();
            }

            return calendarioService.Excluir(id) ? Ok() : NotFound();
        }
    }
}