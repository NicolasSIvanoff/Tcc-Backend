using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TccBackend.Migrations
{
    /// <inheritdoc />
    public partial class PopulaQuizz : Migration
    {
            /// <inheritdoc />
            protected override void Up(MigrationBuilder migrationBuilder)
            {
                migrationBuilder.Sql("Insert into Quizzes(IdQuiz,pontuacao,RespostasQuizIdRespostaQuiz)" +
                    "Values(1,200, 1)");
            }
            protected override void Down(MigrationBuilder migrationBuilder)
            {
                migrationBuilder.Sql("Delete from Quizzes where IdQuiz = 1");
            }
        }
    }
