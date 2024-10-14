namespace TccBackend.Models
{
    public class Resposta
    {
        public int IdResposta { get; set; }
        public int opcaoSelec { get; set; }
        public int PerguntaId { get; set; }
        public Pergunta? Pergunta { get; set; }
    }
}
