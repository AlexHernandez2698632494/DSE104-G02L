using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCPelicula.Migrations
{
    /// <inheritdoc />
    public partial class Pelicula_Col_Director_Sala : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sala",
                table: "Peliculas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sala",
                table: "Peliculas");
        }
    }
}
