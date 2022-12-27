using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiAutores.Migrations
{
    /// <inheritdoc />
    public partial class LibrosRelacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Libros",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    titulo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    autorid = table.Column<int>(name: "autor_id", type: "int", nullable: false),
                    Autorid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libros", x => x.id);
                    table.ForeignKey(
                        name: "FK_Libros_Autores_Autorid",
                        column: x => x.Autorid,
                        principalTable: "Autores",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Libros_Autorid",
                table: "Libros",
                column: "Autorid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Libros");
        }
    }
}
