using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ControlInventariosAPI.Migrations
{
    public partial class AddedIngresos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticulosIngresos",
                columns: table => new
                {
                    IdIngreso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrecioCompra = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    FechaIngreso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClaveArticulo = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    IdProveedor = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticulosIngresos", x => x.IdIngreso);
                    table.ForeignKey(
                        name: "FK_ArticulosIngresos_Articulos_ClaveArticulo",
                        column: x => x.ClaveArticulo,
                        principalTable: "Articulos",
                        principalColumn: "ClaveArticulo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticulosIngresos_Proveedores_IdProveedor",
                        column: x => x.IdProveedor,
                        principalTable: "Proveedores",
                        principalColumn: "IdProveedor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticulosIngresos_ClaveArticulo",
                table: "ArticulosIngresos",
                column: "ClaveArticulo");

            migrationBuilder.CreateIndex(
                name: "IX_ArticulosIngresos_IdProveedor",
                table: "ArticulosIngresos",
                column: "IdProveedor");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticulosIngresos");
        }
    }
}
