namespace TccBackend.Models
{
    public class Quiz
    {

        private RespostasQuiz? respostasQuiz;

        public int IdQuiz { get; set; }
        public int Pontuacao { get; set; }
        public ICollection<Pergunta>? Perguntas { get; set; } = new List<Pergunta>();
        public RespostasQuiz? RespostasQuiz { get => respostasQuiz; set => respostasQuiz = value; }
    }
}
