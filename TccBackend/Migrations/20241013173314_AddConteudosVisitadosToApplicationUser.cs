using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TccBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddConteudosVisitadosToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConteudosVisitados",
                table: "AspNetUsers",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConteudosVisitados",
                table: "AspNetUsers");
        }
    }
}
