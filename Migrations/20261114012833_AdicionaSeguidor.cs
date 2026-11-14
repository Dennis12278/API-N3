using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DreamApi.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaSeguidor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Seguidores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdSeguidor = table.Column<int>(type: "int", nullable: false),
                    IdSeguido = table.Column<int>(type: "int", nullable: false),
                    DataSeguiu = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seguidores", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Seguidores");
        }
    }
}
