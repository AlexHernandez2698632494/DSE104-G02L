using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibroAPI.Migrations
{
    /// <inheritdoc />
    public partial class Migracióninicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "libros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Autor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AnioPublicacion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_libros", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "libros",
                columns: new[] { "Id", "AnioPublicacion", "Autor", "Titulo" },
                values: new object[,]
                {
                    { 1, 1967, "Gabriel Garcia Marquez", "Cien años de soledad" },
                    { 2, 1605, "Miguel de Cervnates", "Don Quijote de la Mancha" },
                    { 3, 1985, "Gabriel García Márquez", "El amor en los tiempos del cólera " }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "libros");
        }
    }
}
