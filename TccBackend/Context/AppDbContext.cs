using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TccBackend.Models;

namespace TccBackend.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :
            base(options)
        {
        }

        public DbSet<Conteudo>? Conteudos { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Pergunta> Perguntas { get; set; }
        public DbSet<Resposta> Respostas { get; set; }
        public DbSet<RespostasQuiz> RespostasQuizzes { get; set; }
        public DbSet<Opcao> Opcoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers", "dbo");

            modelBuilder.Entity<Pergunta>()
                .HasKey(p => p.IdPergunta);

            modelBuilder.Entity<Quiz>()
                .HasKey(q => q.IdQuiz);

            modelBuilder.Entity<Resposta>()
                .HasKey(r => r.IdResposta);

            modelBuilder.Entity<RespostasQuiz>()
                .HasKey(rq => rq.IdRespostaQuiz);

            modelBuilder.Entity<Opcao>()
                .HasKey(o => o.IdOpcao);

            // Configurar a relação entre Resposta e Pergunta
            modelBuilder.Entity<Resposta>()
                .HasOne(r => r.Pergunta)
                .WithMany(p => p.Respostas)
                .HasForeignKey(r => r.PerguntaId);

            // Configurar a relação entre Pergunta e Quiz
            modelBuilder.Entity<Pergunta>()
                .HasOne(p => p.Quiz)
                .WithMany(q => q.Perguntas)
                .HasForeignKey(p => p.QuizId);

            // Configurar a relação entre Opcao e Pergunta
            modelBuilder.Entity<Opcao>()
                .HasOne(o => o.Pergunta)
                .WithMany(p => p.Opcoes)
                .HasForeignKey(o => o.PerguntaId);

            modelBuilder.Entity<Quiz>()
                .HasMany(q => q.Perguntas)
                .WithOne(p => p.Quiz)
                .HasForeignKey(p => p.QuizId);
        }
    }
}
