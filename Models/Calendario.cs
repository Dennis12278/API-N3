namespace DreamApi.Models
{
    public class Calendario
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }

        // início do evento
        public DateTime Data { get; set; }

        public DateTime? DataFim { get; set; }

        // título do evento
        public string Descricao { get; set; } = "";

        // detalhes ou anotações do evento
        public string? Detalhes { get; set; }
    }
}