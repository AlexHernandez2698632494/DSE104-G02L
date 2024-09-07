using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Empleados.Migrations
{
    /// <inheritdoc />
    public partial class empleados_fk_asignaciones_proyectos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Empleados",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "asignacionId",
                table: "Empleados",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "asignacionesId",
                table: "Empleados",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "proyectosId",
                table: "Empleados",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Asignaciones",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaAsignacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    rol = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asignaciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Proyectos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proyectos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_asignacionesId",
                table: "Empleados",
                column: "asignacionesId");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_proyectosId",
                table: "Empleados",
                column: "proyectosId");

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Asignaciones_asignacionesId",
                table: "Empleados",
                column: "asignacionesId",
                principalTable: "Asignaciones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Proyectos_proyectosId",
                table: "Empleados",
                column: "proyectosId",
                principalTable: "Proyectos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Asignaciones_asignacionesId",
                table: "Empleados");

            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Proyectos_proyectosId",
                table: "Empleados");

            migrationBuilder.DropTable(
                name: "Asignaciones");

            migrationBuilder.DropTable(
                name: "Proyectos");

            migrationBuilder.DropIndex(
                name: "IX_Empleados_asignacionesId",
                table: "Empleados");

            migrationBuilder.DropIndex(
                name: "IX_Empleados_proyectosId",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "asignacionId",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "asignacionesId",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "proyectosId",
                table: "Empleados");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Empleados",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);
        }
    }
}
