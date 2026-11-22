namespace DreamApi.Models
{
    public class MembroGrupoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "";
    }

    public class GrupoDetalheDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public string Descricao { get; set; } = "";
        public int Titulos { get; set; }
        public bool Seguindo { get; set; }
        public List<MembroGrupoDto> Membros { get; set; } = new();
    }
}