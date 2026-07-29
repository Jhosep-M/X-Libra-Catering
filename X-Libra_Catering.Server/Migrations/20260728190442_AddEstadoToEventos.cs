using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace X_Libra_Catering.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddEstadoToEventos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ESTADO",
                table: "EVENTOS",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: false,
                defaultValue: "Pendiente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ESTADO",
                table: "EVENTOS");
        }
    }
}
