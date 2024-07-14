using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TccBackend.Context;
using TccBackend.Models;
using Microsoft.AspNetCore.Authorization;


namespace TccBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ConteudosController : ControllerBase
    {
        private readonly AppDbContext _context;

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

        [Authorize]
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
        [HttpGet("{id:int}", Name="ObterProduto")]
        public ActionResult<Conteudo> Get(int id)
        {
            try
            {
                var conteudo = _context?.Conteudos?.FirstOrDefault(c => c.Id == id);
                if (conteudo is null)
                {
                    return NotFound("Conteudo não encontrado");
                }
                return conteudo;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Conteudo não encontrado");
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
    }
}
