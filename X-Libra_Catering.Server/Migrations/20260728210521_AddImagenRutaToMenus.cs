using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace X_Libra_Catering.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddImagenRutaToMenus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IMAGEN_RUTA",
                table: "MENUS",
                type: "varchar(500)",
                unicode: false,
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IMAGEN_RUTA",
                table: "MENUS");
        }
    }
}
