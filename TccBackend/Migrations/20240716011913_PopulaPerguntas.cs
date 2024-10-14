using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TccBackend.Migrations
{
    /// <inheritdoc />
    public partial class PopulaPerguntas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Insert into Perguntas (IdPergunta,Enunciado,RespostaCorreta,Tipo,Pontuacao,QuizId) " +
                               "VALUES(1,'O que é a taxa Selic?','1',1,40,1)");

            migrationBuilder.Sql("INSERT INTO Perguntas (IdPergunta, Enunciado, RespostaCorreta, Tipo, Pontuacao, QuizId) " +
                               "VALUES (2, 'O que é uma ação?','3',1, 40, 1)");

            migrationBuilder.Sql("INSERT INTO Perguntas (IdPergunta, Enunciado, RespostaCorreta, Tipo, Pontuacao, QuizId) " +
                               "VALUES (3, 'O que são Fundos de Investimento Imobiliário (FIIs)?', '2',1, 40, 1)");

            migrationBuilder.Sql("INSERT INTO Perguntas (IdPergunta, Enunciado, RespostaCorreta, Tipo, Pontuacao, QuizId) " +
                               "VALUES (4, 'O que é a taxa CDI?', '2',1, 40, 1)");

            migrationBuilder.Sql("INSERT INTO Perguntas (IdPergunta, Enunciado, RespostaCorreta, Tipo, Pontuacao, QuizId) " +
                               "VALUES (5, 'O que é o mercado de ações?', '1', 1, 40, 1)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Perguntas WHERE IdPergunta = 1");
        }
    }
}
