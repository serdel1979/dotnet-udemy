using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiAutores.Migrations
{
    public partial class Fechapublicaciondelibrocorregido : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fechaPublicacion",
                table: "Autores");

            migrationBuilder.AddColumn<DateTime>(
                name: "fechaPublicacion",
                table: "Libros",
                type: "timestamp with time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fechaPublicacion",
                table: "Libros");

            migrationBuilder.AddColumn<DateTime>(
                name: "fechaPublicacion",
                table: "Autores",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
