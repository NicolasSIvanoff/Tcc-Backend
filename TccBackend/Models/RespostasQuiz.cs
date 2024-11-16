using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TccBackend.Models
{
    public class RespostasQuiz
    {
        public RespostasQuiz()
        {
            Respostas = new Collection<Resposta>();
        }

        public int IdRespostaQuiz { get; set; }
        public int IdQuiz { get; set; }
        public int IdPergunta { get; set; }
        public int Pontuacao { get; set; }
        public DateTime Data { get; set; }

        public ApplicationUser? User { get; set; }

        [Column("UserId")]
        public string UserId { get; set; }
        public ICollection<Resposta> Respostas { get; set; } = new List<Resposta>();
    }
}