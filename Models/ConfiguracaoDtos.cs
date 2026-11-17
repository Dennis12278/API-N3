namespace DreamApi.Models
{
    public class PreferenciasDto
    {
        public bool PerfilPublico { get; set; }
        public bool MostrarPublicacoes { get; set; }
        public bool MostrarEstatisticas { get; set; }
    }

    public class TrocarSenhaDto
    {
        public string SenhaAtual { get; set; } = "";
        public string NovaSenha { get; set; } = "";
    }
}