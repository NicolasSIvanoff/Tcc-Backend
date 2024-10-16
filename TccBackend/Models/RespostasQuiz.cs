﻿using System.Collections.ObjectModel;

namespace TccBackend.Models
{
    public class RespostasQuiz
    {
        public RespostasQuiz() { 
            Respostas = new Collection<Resposta>();
        }
        public int IdRespostaQuiz { get; set; }
        public int IdQuiz { get; set; }
        public int IdPergunta { get; set; }
        public int Pontuacao { get; set; }
        public DateTime data { get; set; }
        public required ICollection<Resposta> Respostas { get; set; } = new List<Resposta>();
    }
}
 