namespace TccBackend.Models
{
    public class RespostasQuizViewModel
    {
        public int IdQuiz { get; set; }
        public List<RespostaViewModel> Respostas { get; set; }
        public int Pontuacao { get; set; }
        public DateTime Data { get; set; }
    }

    public class RespostaViewModel
    {
        public int PerguntaId { get; set; }
        public int opcaoSelec { get; set; }
    }
}