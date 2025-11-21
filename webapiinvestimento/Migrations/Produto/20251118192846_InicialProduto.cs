using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapiinvestimento.Migrations.Produto
{
    /// <inheritdoc />
    public partial class InicialProduto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "produtoInvestimento",
                columns: table => new
                {
                    ID_PRODUTO = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NOME_PRODUTO = table.Column<string>(type: "TEXT", nullable: true),
                    TIPO_PRODUTO = table.Column<string>(type: "TEXT", nullable: true),
                    RISCO_PRODUTO = table.Column<string>(type: "TEXT", nullable: true),
                    RENTABILIDADE = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_produtoInvestimento", x => x.ID_PRODUTO);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "produtoInvestimento");
        }
    }
}
