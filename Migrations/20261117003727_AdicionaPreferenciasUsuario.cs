using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DreamApi.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaPreferenciasUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MostrarEstatisticas",
                table: "Usuarios",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MostrarPublicacoes",
                table: "Usuarios",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PerfilPublico",
                table: "Usuarios",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MostrarEstatisticas",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "MostrarPublicacoes",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "PerfilPublico",
                table: "Usuarios");
        }
    }
}
