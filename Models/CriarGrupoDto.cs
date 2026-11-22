namespace DreamApi.Models
{
    public class CriarGrupoDto
    {
        public string Nome { get; set; } = "";
        public string? Descricao { get; set; }
        public int IdUsuario { get; set; }
    }
}