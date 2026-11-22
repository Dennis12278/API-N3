using DreamApi.Data;
using DreamApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamApi.Services
{
    public class GrupoCriacaoService
    {
        private DreamDbContext context;

        public GrupoCriacaoService(DreamDbContext context)
        {
            this.context = context;
        }

        // cria o grupo e já faz quem criou passar a seguir
        public int Criar(CriarGrupoDto dto)
        {
            Grupo grupo = new Grupo
            {
                Nome = dto.Nome.Trim(),
                Descricao = dto.Descricao ?? ""
            };

            context.Grupos.Add(grupo);
            context.SaveChanges();

            context.GruposUsuarios.Add(new GrupoUsuario
            {
                IdGrupo = grupo.Id,
                IdUsuario = dto.IdUsuario
            });
            context.SaveChanges();

            return grupo.Id;
        }

        public GrupoDetalheDto? Obter(int id, int idUsuario)
        {
            Grupo? grupo = context.Grupos.AsNoTracking().FirstOrDefault(g => g.Id == id);

            if (grupo == null)
            {
                return null;
            }

            List<int> idsMembros = context.GruposUsuarios
                .AsNoTracking()
                .Where(gu => gu.IdGrupo == id)
                .Select(gu => gu.IdUsuario)
                .ToList();

            List<MembroGrupoDto> membros = context.Usuarios
                .AsNoTracking()
                .Where(u => idsMembros.Contains(u.Id))
                .Select(u => new MembroGrupoDto { Id = u.Id, Nome = u.Nome })
                .ToList();

            return new GrupoDetalheDto
            {
                Id = grupo.Id,
                Nome = grupo.Nome,
                Descricao = grupo.Descricao,
                Titulos = context.Compartilhamentos.Count(c => c.IdGrupo == id),
                Seguindo = idsMembros.Contains(idUsuario),
                Membros = membros
            };
        }

        public void Seguir(int id, int idUsuario)
        {
            bool jaSegue = context.GruposUsuarios
                .Any(gu => gu.IdGrupo == id && gu.IdUsuario == idUsuario);

            if (!jaSegue)
            {
                context.GruposUsuarios.Add(new GrupoUsuario
                {
                    IdGrupo = id,
                    IdUsuario = idUsuario
                });

                context.SaveChanges();
            }
        }

        public void Deixar(int id, int idUsuario)
        {
            List<GrupoUsuario> vinculos = context.GruposUsuarios
                .Where(gu => gu.IdGrupo == id && gu.IdUsuario == idUsuario)
                .ToList();

            if (vinculos.Count > 0)
            {
                context.GruposUsuarios.RemoveRange(vinculos);
                context.SaveChanges();
            }
        }
    }
}