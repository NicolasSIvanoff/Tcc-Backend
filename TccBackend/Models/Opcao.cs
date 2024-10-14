using System.Text.Json.Serialization;

namespace TccBackend.Models
{
    public class Opcao
    {
        public int IdOpcao { get; set; } // Chave primária
        public string Letra { get; set; }
        public string Questao { get; set; }
        public int PerguntaId { get; set; }
        [JsonIgnore]
        public Pergunta? Pergunta { get; set; }
    }
}

