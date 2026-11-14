namespace DreamApi.Models
{
    public class UsuarioSeguidoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public string? Avatar { get; set; }
        public int Publicacoes { get; set; }
        public DateTime DataSeguiu { get; set; }
    }
}