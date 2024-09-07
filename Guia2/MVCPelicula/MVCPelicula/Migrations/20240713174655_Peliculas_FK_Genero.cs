using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCPelicula.Migrations
{
    /// <inheritdoc />
    public partial class Peliculas_FK_Genero : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Genero",
                table: "Peliculas");

            migrationBuilder.AddColumn<int>(
                name: "Generold",
                table: "Peliculas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Generos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Generos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Peliculas_Generold",
                table: "Peliculas",
                column: "Generold");

            migrationBuilder.AddForeignKey(
                name: "FK_Peliculas_Generos_Generold",
                table: "Peliculas",
                column: "Generold",
                principalTable: "Generos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Peliculas_Generos_Generold",
                table: "Peliculas");

            migrationBuilder.DropTable(
                name: "Generos");

            migrationBuilder.DropIndex(
                name: "IX_Peliculas_Generold",
                table: "Peliculas");

            migrationBuilder.DropColumn(
                name: "Generold",
                table: "Peliculas");

            migrationBuilder.AddColumn<string>(
                name: "Genero",
                table: "Peliculas",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");
        }
    }
}
