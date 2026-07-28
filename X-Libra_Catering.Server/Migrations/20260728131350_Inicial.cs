using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace X_Libra_Catering.Server.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CLIENTES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOMBRE = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    TELEFONO = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    EMAIL = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    DIRECCION = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLIENTES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MENUS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOMBRE = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    DESCRIPCION = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    CATEGORIA = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    PRECIO = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    REQUIERE_REFRIGERACION = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MENUS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "VEHICULOS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MARCA = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    MODELO = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    PLACA = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    CAPACIDAD_KG = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    TIENE_REFRIGERACION = table.Column<bool>(type: "bit", nullable: false),
                    DISPONIBLE = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VEHICULOS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EVENTOS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CLIENTE_ID = table.Column<int>(type: "int", nullable: false),
                    NOMBRE_EVENTO = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    TIPO_EVENTO = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    FECHA_EVENTO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UBICACION = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    NUM_INVITADOS = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVENTOS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_EVENTOS_CLIENTES",
                        column: x => x.CLIENTE_ID,
                        principalTable: "CLIENTES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PEDIDO_CABECERA",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EVENTO_ID = table.Column<int>(type: "int", nullable: false),
                    VEHICULO_ID = table.Column<int>(type: "int", nullable: false),
                    FECHA_PEDIDO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ESTADO = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    TOTAL = table.Column<decimal>(type: "decimal(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PEDIDO_CABECERA", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PEDIDO_CABECERA_EVENTOS",
                        column: x => x.EVENTO_ID,
                        principalTable: "EVENTOS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PEDIDO_CABECERA_VEHICULOS",
                        column: x => x.VEHICULO_ID,
                        principalTable: "VEHICULOS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PEDIDO_DETALLE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PEDIDO_ID = table.Column<int>(type: "int", nullable: false),
                    MENU_ID = table.Column<int>(type: "int", nullable: false),
                    CANTIDAD = table.Column<int>(type: "int", nullable: false),
                    PRECIO_UNITARIO = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SUBTOTAL = table.Column<decimal>(type: "decimal(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PEDIDO_DETALLE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PEDIDO_DETALLE_MENUS",
                        column: x => x.MENU_ID,
                        principalTable: "MENUS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PEDIDO_DETALLE_PEDIDO_CABECERA",
                        column: x => x.PEDIDO_ID,
                        principalTable: "PEDIDO_CABECERA",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EVENTOS_CLIENTE_ID",
                table: "EVENTOS",
                column: "CLIENTE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PEDIDO_CABECERA_EVENTO_ID",
                table: "PEDIDO_CABECERA",
                column: "EVENTO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PEDIDO_CABECERA_VEHICULO_ID",
                table: "PEDIDO_CABECERA",
                column: "VEHICULO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PEDIDO_DETALLE_MENU_ID",
                table: "PEDIDO_DETALLE",
                column: "MENU_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PEDIDO_DETALLE_PEDIDO_ID",
                table: "PEDIDO_DETALLE",
                column: "PEDIDO_ID");

            migrationBuilder.CreateIndex(
                name: "UQ_VEHICULOS_PLACA",
                table: "VEHICULOS",
                column: "PLACA",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PEDIDO_DETALLE");

            migrationBuilder.DropTable(
                name: "MENUS");

            migrationBuilder.DropTable(
                name: "PEDIDO_CABECERA");

            migrationBuilder.DropTable(
                name: "EVENTOS");

            migrationBuilder.DropTable(
                name: "VEHICULOS");

            migrationBuilder.DropTable(
                name: "CLIENTES");
        }
    }
}
