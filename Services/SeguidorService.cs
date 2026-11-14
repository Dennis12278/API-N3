using DreamApi.Data;
using DreamApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamApi.Services
{
    public class SeguidorService
    {
        private DreamDbContext context;

        public SeguidorService(DreamDbContext context)
        {
            this.context = context;
        }

        // usuários que o usuário segue
        public List<UsuarioSeguidoDto> ListarSeguidos(int idUsuario)
        {
            return context.Seguidores
                .AsNoTracking()
                .Where(s => s.IdSeguidor == idUsuario)
                .Join(
                    context.Usuarios.AsNoTracking(),
                    s => s.IdSeguido,
                    u => u.Id,
                    (s, u) => new UsuarioSeguidoDto
                    {
                        Id = u.Id,
                        Nome = u.Nome,
                        Avatar = u.Avatar,
                        Publicacoes = context.Documentos.Count(d => d.IdUsuario == u.Id && d.Publicado),
                        DataSeguiu = s.DataSeguiu
                    })
                .OrderByDescending(x => x.DataSeguiu)
                .ToList();
        }

        public bool UsuarioExiste(int id)
        {
            return context.Usuarios.AsNoTracking().Any(u => u.Id == id);
        }

        public bool Segue(int idUsuario, int idAlvo)
        {
            return context.Seguidores.Any(s => s.IdSeguidor == idUsuario && s.IdSeguido == idAlvo);
        }

        public bool Seguir(int idUsuario, int idAlvo)
        {
            if (idUsuario == idAlvo || !UsuarioExiste(idAlvo))
            {
                return false;
            }

            if (Segue(idUsuario, idAlvo))
            {
                return true;
            }

            context.Seguidores.Add(new Seguidor
            {
                IdSeguidor = idUsuario,
                IdSeguido = idAlvo,
                DataSeguiu = DateTime.UtcNow
            });

            context.SaveChanges();

            return true;
        }

        public void DeixarDeSeguir(int idUsuario, int idAlvo)
        {
            Seguidor? seguidor = context.Seguidores
                .FirstOrDefault(s => s.IdSeguidor == idUsuario && s.IdSeguido == idAlvo);

            if (seguidor != null)
            {
                context.Seguidores.Remove(seguidor);
                context.SaveChanges();
            }
        }
    }
}