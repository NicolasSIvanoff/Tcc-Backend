using Microsoft.EntityFrameworkCore.Migrations;
using System.Collections.Generic;
using TccBackend.Models;

#nullable disable

namespace TccBackend.Migrations
{
    /// <inheritdoc />
    public partial class PopulaOpcoes : Migration
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
