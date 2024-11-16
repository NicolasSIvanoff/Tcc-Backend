using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TccBackend.Context;
using TccBackend.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace TccBackend.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class ConteudosController : ControllerBase
    {
        private readonly AppDbContext _context;

        private static readonly Dictionary<int, string> OpcaoParaLetra = new()
        {
            { 1, "A" },
            { 2, "B" },
            { 3, "C" },
            { 4, "D" }
        };


        public ConteudosController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<Conteudo>> Get()
        {
            try
            {
                var conteudos = _context?.Conteudos?.Take(3).ToList();
                if (conteudos is null)
                {
                    return NotFound("Conteudos não encontrados");
                }
                return conteudos;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao buscar conteudos");
            }
           
        }

        [HttpGet("GetAll")]
        public ActionResult<IEnumerable<Conteudo>> GetAll()
        {
            try
            {
                var conteudos = _context?.Conteudos?.ToList();
                if (conteudos is null)
                {
                    return NotFound("Conteudos não encontrados");
                }
                return conteudos;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao buscar conteudos");
            }

        }

        [Authorize]
        [HttpGet("{id:int}", Name = "ObterProduto")]
        public async Task<ActionResult<Conteudo>> Get(int id)
        {
            try
            {

                var conteudo = _context?.Conteudos?.FirstOrDefault(c => c.Id == id);
                if (conteudo is null)
                {
                    return NotFound("Conteudo não encontrado");
                }

                var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("Usuário não autenticado");
                }

                var usuario = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
                if (usuario is null)
                {
                    return NotFound("Usuário não encontrado");
                }

                if (!usuario.ConteudosVisitados.Contains(id))
                {
                    usuario.ConteudosVisitados.Add(id);
                    _context.Users.Update(usuario);
                    await _context.SaveChangesAsync();
                }

                return conteudo;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao buscar conteúdo");
            }
        }



        [Authorize]
        [HttpPost]
        public ActionResult<Conteudo> Post([FromBody] Conteudo conteudo)
        {
            if(conteudo is null)
            {
                return BadRequest("Conteudo inválido");
            }
            _context?.Conteudos?.Add(conteudo);
            _context?.SaveChanges();
            return new CreatedAtActionResult("ObterConteudo",
                "Conteudos", new { id = conteudo.Id }, conteudo);
        }


        [Authorize]
        [HttpPut("{id:int}")]
        public ActionResult<Conteudo> Put(int id, [FromBody] Conteudo conteudo)
        {
            if(conteudo is null || conteudo.Id != id)
            {
                return BadRequest("Conteudo inválido");
            }
            var conteudoAtual = _context?.Conteudos?.FirstOrDefault(c => c.Id == id);
            if(conteudoAtual is null)
            {
                return NotFound("Conteudo não encontrado");
            }
            conteudoAtual.Fontes = conteudo.Fontes;
            conteudoAtual.Data = conteudo.Data;
            conteudoAtual.Criador = conteudo.Criador;
            conteudoAtual.Dados = conteudo.Dados;
            conteudoAtual.Imagem = conteudo.Imagem;
            conteudoAtual.Titulo = conteudo.Titulo;
            _context?.SaveChanges();
            return conteudoAtual;
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var conteudo = _context?.Conteudos?.FirstOrDefault(c => c.Id == id);
            if(conteudo is null)
            {
                return NotFound("Conteudo não encontrado");
            }
            _context?.Conteudos?.Remove(conteudo);
            _context?.SaveChanges();
            return Ok(conteudo);
        }
        [Authorize]
        [HttpGet("conteudos-visitados", Name = "ObterConteudosVisitados")]
        public async Task<ActionResult<List<Conteudo>>> GetConteudosVisitados()
        {
            try
            {
                var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("Usuário não autenticado");
                }

                var usuario = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserName == userName);

                if (usuario is null)
                {
                    return NotFound("Usuário não encontrado");
                }

                var conteudosVisitados = await _context.Conteudos
                    .Where(c => usuario.ConteudosVisitados.Contains(c.Id))
                    .ToListAsync();

                return Ok(conteudosVisitados);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao buscar conteúdos visitados");
            }
        }
        [HttpPost("salvarResultadoQuiz")]
        public async Task<IActionResult> SalvarResultadoQuiz([FromBody] RespostasQuizViewModel respostasQuizViewModel)
        {
            // Obter o email do usuário autenticado
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized("Usuário não autenticado.");
            }

            // Obter o usuário pelo email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null)
            {
                return Unauthorized("Usuário não encontrado.");
            }

            // Preparar os dados para a validação e cálculo
            var respostasUsuario = respostasQuizViewModel.Respostas.Select(r => new UsuarioResposta
            {
                IdPergunta = r.PerguntaId,
                RespostaUsuario = OpcaoParaLetra[r.opcaoSelec]
            }).ToList();

            // Obter o resultado do quiz utilizando a lógica já existente
            var resultadoQuiz = ObterResultadoQuiz(respostasQuizViewModel.IdQuiz, respostasUsuario) as OkObjectResult;

            if (resultadoQuiz == null || resultadoQuiz.Value == null)
            {
                return BadRequest("Erro ao calcular o resultado do quiz.");
            }

            var resultado = resultadoQuiz.Value as dynamic;

            // Criar o modelo de RespostasQuiz para salvar no banco
            var respostasQuiz = new RespostasQuiz
            {
                IdQuiz = respostasQuizViewModel.IdQuiz,
                UserId = user.Id,
                Respostas = respostasQuizViewModel.Respostas.Select(r => new Resposta
                {
                    PerguntaId = r.PerguntaId,
                    opcaoSelec = r.opcaoSelec
                }).ToList(),
                Pontuacao = resultado.Pontuacao,
                Data = DateTime.UtcNow
            };

            // Salvar o resultado no banco de dados
            _context.RespostasQuizzes.Add(respostasQuiz);
            await _context.SaveChangesAsync();

            // Retornar feedback ao usuário com as respostas erradas
            return Ok(new
            {
                message = "Resultado do quiz salvo com sucesso.",
                pontuacao = resultado.Pontuacao,
                perguntasErradas = resultado.PerguntasErradas
            });
        }


        [HttpGet("obterResultadoQuiz")]
        public IActionResult ObterResultadoQuiz(int idQuiz, List<UsuarioResposta> respostasUsuario)
        {
            // Validação inicial
            if (respostasUsuario == null || !respostasUsuario.Any())
                return BadRequest("Nenhuma resposta foi enviada pelo usuário.");

            // Busca o quiz com suas perguntas e opções
            var quiz = _context.Quizzes
                .Include(q => q.Perguntas)
                .ThenInclude(p => p.Opcoes)
                .FirstOrDefault(q => q.IdQuiz == idQuiz);

            if (quiz == null)
                return NotFound("Quiz não encontrado");

            // Identifica perguntas respondidas incorretamente
            var perguntasErradas = quiz.Perguntas
                .Where(p => respostasUsuario.Any(r =>
                    r.IdPergunta == p.IdPergunta && r.RespostaUsuario != p.RespostaCorreta))
                .Select(p => new
                {
                    p.IdPergunta,
                    p.Enunciado,
                    p.RespostaCorreta,
                    p.Tipo,
                    p.Pontuacao,
                    p.QuizId,
                    Opcoes = p.Opcoes.Select(o => new
                    {
                        o.IdOpcao,
                        o.Letra,
                        o.Questao,
                        o.PerguntaId
                    }),
                    Respostas = respostasUsuario
                        .Where(r => r.IdPergunta == p.IdPergunta)
                        .Select(r => new
                        {
                            r.IdOpcaoSelecionada,
                            r.RespostaUsuario
                        })
                }).ToList();

            // Calcula a pontuação total
            var pontuacao = respostasUsuario.Sum(r =>
                quiz.Perguntas.Any(p =>
                    p.IdPergunta == r.IdPergunta && p.RespostaCorreta == r.RespostaUsuario) ?
                    quiz.Perguntas.First(p => p.IdPergunta == r.IdPergunta).Pontuacao : 0);

            // Retorna o resultado
            return Ok(new
            {
                IdQuiz = quiz.IdQuiz,
                Pontuacao = pontuacao,
                PerguntasErradas = perguntasErradas
            });
        }




        private int CalcularPontuacao(RespostasQuiz respostasQuiz, Quiz quiz)
        {
            int pontuacaoTotal = 0;
            foreach (var resposta in respostasQuiz.Respostas)
            {
                var pergunta = quiz.Perguntas.FirstOrDefault(p => p.IdPergunta == resposta.PerguntaId);
                if (pergunta != null && pergunta.RespostaCorreta == resposta.opcaoSelec.ToString())
                {
                    pontuacaoTotal += pergunta.Pontuacao;
                }
            }
            return pontuacaoTotal;
        }


    }
}
