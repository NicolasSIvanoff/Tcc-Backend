namespace TccBackend.Models
{
    public class Pergunta
    {
        public int IdPergunta { get; set; }
        public string Enunciado { get; set; } 
        public List<string> Opcoes { get; set; } = new List<string>();
        public int Resposta { get; set; }
        public int Tipo { get; set; }  
        public int Pontuacao { get; set; }
        public List<Conteudo> ConteudoRelacionado { get; set; } = new List<Conteudo>();
    }
}
