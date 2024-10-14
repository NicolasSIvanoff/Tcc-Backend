using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TccBackend.Migrations
{
    /// <inheritdoc />
    public partial class PopulaQuestoess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Opcoes (Letra, Questao, PerguntaId) VALUES ('A', 'É a taxa básica de juros da economia, que influencia outras taxas de juros do país', 1)");
            migrationBuilder.Sql("INSERT INTO Opcoes (Letra, Questao, PerguntaId) VALUES ('B', 'É uma taxa de juros utilizada nos empréstimos entre os bancos', 1)");
            migrationBuilder.Sql("INSERT INTO Opcoes (Letra, Questao, PerguntaId) VALUES ('C', 'É uma taxa sobre o lucro líquido das companhias de capital aberto', 1)");

            migrationBuilder.Sql("INSERT INTO Opcoes (Letra, Questao, PerguntaId) VALUES ('C', 'Uma ação representa uma fração do capital social de uma empresa, conferindo a seu titular participação nos lucros e nos riscos do negócio', 2)");
            migrationBuilder.Sql("INSERT INTO Opcoes (Letra, Questao, PerguntaId) VALUES ('B', 'Uma ação é um título de renda fixa que paga juros ao longo do tempo', 2)");
            migrationBuilder.Sql("INSERT INTO Opcoes (Letra, Questao, PerguntaId) VALUES ('A', 'Uma ação é um título de dívida emitido por empresas para financiar suas operações', 2)");

            migrationBuilder.Sql("INSERT INTO Opcoes (Letra, Questao, PerguntaId) VALUES ('A', 'FIIs são fundos de investimento em ações de empresas do setor imobiliário', 3)");
            migrationBuilder.Sql("INSERT INTO Opcoes (Letra, Questao, PerguntaId) VALUES ('B', 'FIIs são fundos de investimento imobiliário que aplicam em empreendimentos imobiliários', 3)");
            migrationBuilder.Sql("INSERT INTO Opcoes (Letra, Questao, PerguntaId) VALUES ('C', 'FIIs são fundos de investimento em títulos públicos', 3)");

            migrationBuilder.Sql("INSERT INTO Opcoes (Letra, Questao, PerguntaId) VALUES ('A', 'A taxa CDI é a taxa de retorno dos investimentos em ações', 4)");
            migrationBuilder.Sql("INSERT INTO Opcoes (Letra, Questao, PerguntaId) VALUES ('B', 'A taxa CDI (Certificado de Depósito Interbancário) é a taxa média dos empréstimos entre instituições financeiras', 4)");
            migrationBuilder.Sql("INSERT INTO Opcoes (Letra, Questao, PerguntaId) VALUES ('C', 'A taxa CDI é a taxa de inflação acumulada no período', 4)");

            migrationBuilder.Sql("INSERT INTO Opcoes (Letra, Questao, PerguntaId) VALUES ('A', 'O mercado de ações é onde se compram e vendem ações de empresas de capital aberto', 5)");
            migrationBuilder.Sql("INSERT INTO Opcoes (Letra, Questao, PerguntaId) VALUES ('B', 'O mercado de ações é onde se compram e vendem títulos de dívida de empresas', 5)");
            migrationBuilder.Sql("INSERT INTO Opcoes (Letra, Questao, PerguntaId) VALUES ('C', 'O mercado de ações é onde se compram e vendem imóveis comerciais', 5)");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
