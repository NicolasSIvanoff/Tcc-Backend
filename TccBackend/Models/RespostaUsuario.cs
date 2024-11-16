namespace TccBackend.Models
{
    public class UsuarioResposta
    {
        public int IdPergunta { get; set; }
        public string RespostaUsuario { get; set; }
        public int? IdOpcaoSelecionada { get; set; } // Referência opcional à opção selecionada
    }
}
