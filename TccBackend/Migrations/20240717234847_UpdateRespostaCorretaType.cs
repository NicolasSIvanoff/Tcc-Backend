using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TccBackend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRespostaCorretaType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "pontuacao",
                table: "Quizzes",
                newName: "Pontuacao");

            migrationBuilder.AlterColumn<string>(
                name: "RespostaCorreta",
                table: "Perguntas",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Pontuacao",
                table: "Quizzes",
                newName: "pontuacao");

            migrationBuilder.AlterColumn<int>(
                name: "RespostaCorreta",
                table: "Perguntas",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
