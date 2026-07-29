using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace X_Libra_Catering.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ACTIVO",
                table: "VEHICULOS",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "ACTIVO",
                table: "MENUS",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "EVENTOS",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ACTIVO",
                table: "CLIENTES",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ACTIVO",
                table: "VEHICULOS");

            migrationBuilder.DropColumn(
                name: "ACTIVO",
                table: "MENUS");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "EVENTOS");

            migrationBuilder.DropColumn(
                name: "ACTIVO",
                table: "CLIENTES");
        }
    }
}
