using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace X_Libra_Catering.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FECHA_CREACION",
                table: "VEHICULOS",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FECHA_MODIFICACION",
                table: "VEHICULOS",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FECHA_CREACION",
                table: "PEDIDO_DETALLE",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FECHA_MODIFICACION",
                table: "PEDIDO_DETALLE",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FECHA_CREACION",
                table: "PEDIDO_CABECERA",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FECHA_MODIFICACION",
                table: "PEDIDO_CABECERA",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FECHA_CREACION",
                table: "MENUS",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FECHA_MODIFICACION",
                table: "MENUS",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FECHA_CREACION",
                table: "EVENTOS",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FECHA_MODIFICACION",
                table: "EVENTOS",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FECHA_CREACION",
                table: "CLIENTES",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FECHA_MODIFICACION",
                table: "CLIENTES",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FECHA_CREACION",
                table: "VEHICULOS");

            migrationBuilder.DropColumn(
                name: "FECHA_MODIFICACION",
                table: "VEHICULOS");

            migrationBuilder.DropColumn(
                name: "FECHA_CREACION",
                table: "PEDIDO_DETALLE");

            migrationBuilder.DropColumn(
                name: "FECHA_MODIFICACION",
                table: "PEDIDO_DETALLE");

            migrationBuilder.DropColumn(
                name: "FECHA_CREACION",
                table: "PEDIDO_CABECERA");

            migrationBuilder.DropColumn(
                name: "FECHA_MODIFICACION",
                table: "PEDIDO_CABECERA");

            migrationBuilder.DropColumn(
                name: "FECHA_CREACION",
                table: "MENUS");

            migrationBuilder.DropColumn(
                name: "FECHA_MODIFICACION",
                table: "MENUS");

            migrationBuilder.DropColumn(
                name: "FECHA_CREACION",
                table: "EVENTOS");

            migrationBuilder.DropColumn(
                name: "FECHA_MODIFICACION",
                table: "EVENTOS");

            migrationBuilder.DropColumn(
                name: "FECHA_CREACION",
                table: "CLIENTES");

            migrationBuilder.DropColumn(
                name: "FECHA_MODIFICACION",
                table: "CLIENTES");
        }
    }
}
