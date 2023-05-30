using Microsoft.EntityFrameworkCore.Migrations;

namespace ControlInventariosAPI.Migrations
{
    public partial class AddedEgresos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticuloEgresos",
                columns: table => new
                {
                    IdEgreso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrecioVenta = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    ClaveArticulo = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    IdPedido = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticuloEgresos", x => x.IdEgreso);
                    table.ForeignKey(
                        name: "FK_ArticuloEgresos_Articulos_ClaveArticulo",
                        column: x => x.ClaveArticulo,
                        principalTable: "Articulos",
                        principalColumn: "ClaveArticulo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticuloEgresos_Pedidos_IdPedido",
                        column: x => x.IdPedido,
                        principalTable: "Pedidos",
                        principalColumn: "IdPedido",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticuloEgresos_ClaveArticulo",
                table: "ArticuloEgresos",
                column: "ClaveArticulo");

            migrationBuilder.CreateIndex(
                name: "IX_ArticuloEgresos_IdPedido",
                table: "ArticuloEgresos",
                column: "IdPedido");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticuloEgresos");
        }
    }
}
