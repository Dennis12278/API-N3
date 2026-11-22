using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DreamApi.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaArquivoLeitura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArquivoLeitura",
                table: "Documentos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoArquivo",
                table: "Documentos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArquivoLeitura",
                table: "Documentos");

            migrationBuilder.DropColumn(
                name: "TipoArquivo",
                table: "Documentos");
        }
    }
}
