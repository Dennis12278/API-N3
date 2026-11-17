using DreamApi.Models;
using DreamApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DreamApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PerfilController : ControllerBase
    {
        private PerfilService perfilService;

        public PerfilController(PerfilService perfilService)
        {
            this.perfilService = perfilService;
        }

        [HttpGet("{id:int}/Publico")]
        public IActionResult Publico(int id)
        {
            PerfilPublicoDto? perfil = perfilService.ObterPublico(id);

            return perfil == null ? NotFound() : Ok(perfil);
        }

        [HttpGet("{id:int}/Meu")]
        public IActionResult Meu(int id)
        {
            PerfilMeuDto? perfil = perfilService.ObterMeu(id);

            return perfil == null ? NotFound() : Ok(perfil);
        }

        [HttpPut("{id:int}")]
        public IActionResult Salvar(int id, [FromBody] PerfilSalvarDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome))
            {
                return BadRequest("Informe o nome.");
            }

            if (string.IsNullOrWhiteSpace(dto.Email) || !dto.Email.Contains('@'))
            {
                return BadRequest("Informe um e-mail válido.");
            }

            return perfilService.Salvar(id, dto) ? Ok() : NotFound();
        }

        [HttpGet("{id:int}/Preferencias")]
        public IActionResult Preferencias(int id)
        {
            PreferenciasDto? dados = perfilService.ObterPreferencias(id);

            return dados == null ? NotFound() : Ok(dados);
        }

        [HttpPut("{id:int}/Preferencias")]
        public IActionResult SalvarPreferencias(int id, [FromBody] PreferenciasDto dto)
        {
            return perfilService.SalvarPreferencias(id, dto) ? Ok() : NotFound();
        }

        [HttpPut("{id:int}/Senha")]
        public IActionResult TrocarSenha(int id, [FromBody] TrocarSenhaDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.NovaSenha) || dto.NovaSenha.Length < 6)
            {
                return BadRequest("A nova senha precisa ter pelo menos 6 caracteres.");
            }

            int resultado = perfilService.TrocarSenha(id, dto);

            if (resultado == 1)
            {
                return NotFound();
            }

            if (resultado == 2)
            {
                return BadRequest("A senha atual está incorreta.");
            }

            return Ok();
        }
    }
}