using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace X_Libra_Catering.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddDireccionLatLngToVehiculos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DIRECCION",
                table: "VEHICULOS",
                type: "varchar(200)",
                unicode: false,
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "LATITUD",
                table: "VEHICULOS",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "LONGITUD",
                table: "VEHICULOS",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DIRECCION",
                table: "VEHICULOS");

            migrationBuilder.DropColumn(
                name: "LATITUD",
                table: "VEHICULOS");

            migrationBuilder.DropColumn(
                name: "LONGITUD",
                table: "VEHICULOS");
        }
    }
}
