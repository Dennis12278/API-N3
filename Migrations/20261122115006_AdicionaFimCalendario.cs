using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DreamApi.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaFimCalendario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataFim",
                table: "Calendarios",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Detalhes",
                table: "Calendarios",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataFim",
                table: "Calendarios");

            migrationBuilder.DropColumn(
                name: "Detalhes",
                table: "Calendarios");
        }
    }
}
