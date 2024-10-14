using System.Text.Json.Serialization;

namespace TccBackend.Models
{
    public class Pergunta
    {
        public int IdPergunta { get; set; }
        public string? Enunciado { get; set; }
        public string RespostaCorreta { get; set; }
        public int Tipo { get; set; }
        public int Pontuacao { get; set; }
        public int QuizId { get; set; }
        [JsonIgnore]
        public Quiz? Quiz { get; set; }
        public ICollection<Opcao> Opcoes { get; set; } = new List<Opcao>();
        public ICollection<Resposta> Respostas { get; set; } = new List<Resposta>();
    }
}

