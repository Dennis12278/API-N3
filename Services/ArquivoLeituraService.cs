using DreamApi.Data;
using DreamApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamApi.Services
{
    public class ArquivoLeituraService
    {
        private readonly DreamDbContext context;
        private readonly IWebHostEnvironment env;

        private static readonly string[] ExtensoesPermitidas = { ".pdf", ".txt" };

        private const long TamanhoMaximo = 20 * 1024 * 1024; // 20 MB

        public ArquivoLeituraService(DreamDbContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }

        private string Pasta()
        {
            // fora de wwwroot: não fica acessível por URL pública
            string pasta = Path.Combine(env.ContentRootPath, "ArquivosLeitura");
            Directory.CreateDirectory(pasta);
            return pasta;
        }

        public string? Validar(IFormFile? arquivo)
        {
            if (arquivo == null || arquivo.Length == 0)
            {
                return "Envie um arquivo PDF ou TXT.";
            }

            string extensao = Path.GetExtension(arquivo.FileName).ToLowerInvariant();

            if (!ExtensoesPermitidas.Contains(extensao))
            {
                return "Formato inválido. Envie um arquivo PDF ou TXT.";
            }

            if (arquivo.Length > TamanhoMaximo)
            {
                return "O arquivo deve ter no máximo 20 MB.";
            }

            return null;
        }

        // retorna null se deu certo, ou a mensagem de erro
        public async Task<string?> Enviar(int idDocumento, int idUsuario, IFormFile arquivo)
        {
            Documento? documento = context.Documentos.Find(idDocumento);

            if (documento == null)
            {
                return "Publicação não encontrada.";
            }

            if (documento.IdUsuario != idUsuario)
            {
                return "Você não pode enviar arquivo para esta publicação.";
            }

            string extensao = Path.GetExtension(arquivo.FileName).ToLowerInvariant();
            string nome = Guid.NewGuid() + extensao;
            string caminho = Path.Combine(Pasta(), nome);

            await using (FileStream stream = File.Create(caminho))
            {
                await arquivo.CopyToAsync(stream);
            }

            string? antigo = documento.ArquivoLeitura;

            documento.ArquivoLeitura = nome;
            documento.TipoArquivo = extensao == ".pdf" ? "pdf" : "txt";

            context.SaveChanges();

            ApagarAntigo(antigo);

            return null;
        }

        public (string caminho, string tipo)? Obter(int idDocumento)
        {
            Documento? documento = context.Documentos
                .AsNoTracking()
                .FirstOrDefault(d => d.Id == idDocumento);

            if (documento == null || string.IsNullOrEmpty(documento.ArquivoLeitura))
            {
                return null;
            }

            string caminho = Path.Combine(Pasta(), documento.ArquivoLeitura);

            if (!File.Exists(caminho))
            {
                return null;
            }

            return (caminho, documento.TipoArquivo ?? "txt");
        }

        private void ApagarAntigo(string? nomeArquivo)
        {
            if (string.IsNullOrEmpty(nomeArquivo))
            {
                return;
            }

            string caminho = Path.Combine(Pasta(), nomeArquivo);

            if (File.Exists(caminho))
            {
                File.Delete(caminho);
            }
        }
    }
}
