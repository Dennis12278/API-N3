using DreamApi.Data;
using DreamApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DreamApi.Services
{
    public class PerfilService
    {
        private DreamDbContext context;
        private readonly PasswordHasher<Usuario> hasher = new PasswordHasher<Usuario>();

        public PerfilService(DreamDbContext context)
        {
            this.context = context;
        }

        private Usuario? Buscar(int id)
        {
            return context.Usuarios.AsNoTracking().FirstOrDefault(u => u.Id == id);
        }

        private int ContarTitulos(int id) =>
            context.MarcadoresPublicacao.Count(m => m.IdUsuario == id);

        private int ContarPublicacoes(int id) =>
            context.Documentos.Count(d => d.IdUsuario == id && d.Publicado);

        private int ContarColecoesPublicas(int id) =>
            context.Colecoes.Count(c => c.IdUsuario == id && c.Visibilidade == "publica");

        private int ContarGrupos(int id) =>
            context.GruposUsuarios.Count(g => g.IdUsuario == id);

        public PerfilPublicoDto? ObterPublico(int id)
        {
            Usuario? usuario = Buscar(id);

            if (usuario == null)
            {
                return null;
            }

            // estatísticas ocultas pelo usuário ficam zeradas
            return new PerfilPublicoDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Biografia = usuario.Biografia ?? "",
                Links = usuario.Links ?? "",
                Avatar = usuario.Avatar,
                Titulos = usuario.MostrarEstatisticas ? ContarTitulos(id) : 0,
                Publicacoes = usuario.MostrarPublicacoes ? ContarPublicacoes(id) : 0,
                Colecoes = usuario.MostrarEstatisticas ? ContarColecoesPublicas(id) : 0,
                Grupos = usuario.MostrarEstatisticas ? ContarGrupos(id) : 0
            };
        }

        public PerfilMeuDto? ObterMeu(int id)
        {
            Usuario? usuario = Buscar(id);

            if (usuario == null)
            {
                return null;
            }

            return new PerfilMeuDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Biografia = usuario.Biografia ?? "",
                Links = usuario.Links ?? "",
                Avatar = usuario.Avatar,
                Titulos = ContarTitulos(id),
                Publicacoes = ContarPublicacoes(id),
                Colecoes = context.Colecoes.Count(c => c.IdUsuario == id),
                Grupos = ContarGrupos(id)
            };
        }

        public bool Salvar(int id, PerfilSalvarDto dto)
        {
            Usuario? usuario = context.Usuarios.Find(id);

            if (usuario == null)
            {
                return false;
            }

            usuario.Nome = dto.Nome.Trim();
            usuario.Email = dto.Email.Trim();
            usuario.Biografia = dto.Biografia ?? "";
            usuario.Links = dto.Links ?? "";

            // só troca a foto se ela foi enviada
            if (dto.Avatar != null)
            {
                usuario.Avatar = dto.Avatar == "" ? null : dto.Avatar;
            }

            context.SaveChanges();

            return true;
        }

        public PreferenciasDto? ObterPreferencias(int id)
        {
            Usuario? usuario = Buscar(id);

            if (usuario == null)
            {
                return null;
            }

            return new PreferenciasDto
            {
                PerfilPublico = usuario.PerfilPublico,
                MostrarPublicacoes = usuario.MostrarPublicacoes,
                MostrarEstatisticas = usuario.MostrarEstatisticas
            };
        }

        public bool SalvarPreferencias(int id, PreferenciasDto dto)
        {
            Usuario? usuario = context.Usuarios.Find(id);

            if (usuario == null)
            {
                return false;
            }

            usuario.PerfilPublico = dto.PerfilPublico;
            usuario.MostrarPublicacoes = dto.MostrarPublicacoes;
            usuario.MostrarEstatisticas = dto.MostrarEstatisticas;

            context.SaveChanges();

            return true;
        }

        
        public int TrocarSenha(int id, TrocarSenhaDto dto)
        {
            Usuario? usuario = context.Usuarios.Find(id);

            if (usuario == null)
            {
                return 1;
            }

            PasswordVerificationResult resultado;

            try
            {
                resultado = hasher.VerifyHashedPassword(usuario, usuario.Senha, dto.SenhaAtual);
            }
            catch (FormatException)
            {
                
                return 2;
            }

            if (resultado == PasswordVerificationResult.Failed)
            {
                return 2;
            }

            usuario.Senha = hasher.HashPassword(usuario, dto.NovaSenha);

            context.SaveChanges();

            return 0;
        }
    }
}