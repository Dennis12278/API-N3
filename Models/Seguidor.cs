namespace DreamApi.Models
{
    public class Seguidor
    {
        public int Id { get; set; }

        // quem segue
        public int IdSeguidor { get; set; }

        // quem é seguido
        public int IdSeguido { get; set; }

        public DateTime DataSeguiu { get; set; }
    }
}