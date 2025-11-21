using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapiinvestimento.Migrations.Simulacao
{
    /// <inheritdoc />
    public partial class InicialSimulacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "simulacoesInvestimentos",
                columns: table => new
                {
                    ID_SIMULACAO = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ID_CLIENTE = table.Column<int>(type: "INTEGER", nullable: false),
                    NOME_PRODUTO = table.Column<string>(type: "TEXT", nullable: true),
                    VALOR_INVESTIMENTO = table.Column<decimal>(type: "TEXT", nullable: false),
                    VALOR_FINAL = table.Column<decimal>(type: "TEXT", nullable: false),
                    PRAZO = table.Column<int>(type: "INTEGER", nullable: false),
                    DATA_SIMULACAO = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_simulacoesInvestimentos", x => x.ID_SIMULACAO);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "simulacoesInvestimentos");
        }
    }
}
