namespace TccBackend.Models
{
    public class Quiz
    {
        public int IdQuiz { get; set; }
        
        public ICollection<Pergunta> Perguntas { get; set; } = new List<Pergunta>();
        public ICollection<RespostasQuiz> RespostasQuiz { get; set; } = new List<RespostasQuiz>();
        public int pontuacao { get; set; }
 
    }
}
