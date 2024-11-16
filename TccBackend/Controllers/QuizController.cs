using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TccBackend.Context;
using TccBackend.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace TccBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class QuizzesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public QuizzesController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quiz>>> Get()
        {
            try
            {
                var quizzes = await _context.Quizzes
                    .Include(q => q.Perguntas)
                    .ToListAsync();
                if (quizzes == null)
                {
                    return NotFound("Quizzes não encontrados");
                }
                return quizzes;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao buscar quizzes");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Quiz>> GetQuiz(int id)
        {
            try
            {
                var quiz = await _context.Quizzes
                    .Include(q => q.Perguntas)
                    .ThenInclude(p => p.Opcoes)
                    .FirstOrDefaultAsync(q => q.IdQuiz == id);

                if (quiz == null)
                {
                    return NotFound("Quiz não encontrado");
                }
                return quiz;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao buscar quiz");
            }
        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Quiz>> Post([FromBody] Quiz quiz)
        {
            if (quiz == null)
            {
                return BadRequest("Quiz inválido");
            }
            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetQuiz), new { id = quiz.IdQuiz }, quiz);
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Quiz>> Put(int id, [FromBody] Quiz quiz)
        {
            if (quiz == null || quiz.IdQuiz != id)
            {
                return BadRequest("Quiz inválido");
            }
            var quizAtual = await _context.Quizzes.FindAsync(id);
            if (quizAtual == null)
            {
                return NotFound("Quiz não encontrado");
            }
            quizAtual.Pontuacao = quiz.Pontuacao;
            quizAtual.Perguntas = quiz.Perguntas;
            quizAtual.RespostasQuiz = quiz.RespostasQuiz;
            await _context.SaveChangesAsync();
            return quizAtual;
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null)
            {
                return NotFound("Quiz não encontrado");
            }
            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();
            return Ok(quiz);
        }

        [Authorize]
        [HttpPost("salvarResultado")]
        public async Task<IActionResult> SaveQuizResultByUserName(string userName, int quizId, int score)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
                if (user == null)
                {
                    return NotFound("Usuário não encontrado");
                }

                var respostasQuiz = new RespostasQuiz
                {
                    IdQuiz = quizId,
                    Pontuacao = score,
                    Data = DateTime.UtcNow,
                    Respostas = new List<Resposta>()
                };

                _context.RespostasQuizzes.Add(respostasQuiz);
                await _context.SaveChangesAsync();

                return Ok("Resultado salvo com sucesso!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao salvar resultado: {ex.Message}");
            }
        }

    }

}
