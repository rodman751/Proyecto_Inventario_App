using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventario.API.Migrations
{
    /// <inheritdoc />
    public partial class Db_act : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AjusteProducto",
                columns: table => new
                {
                    ID_Ajuste = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroAjuste = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Impreso = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AjusteProducto", x => x.ID_Ajuste);
                });

            migrationBuilder.CreateTable(
                name: "Producto",
                columns: table => new
                {
                    ID_Producto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GravaIVA = table.Column<bool>(type: "bit", nullable: false),
                    Costo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PVP = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Estado = table.Column<bool>(type: "bit", maxLength: 20, nullable: false),
                    StockProducto = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producto", x => x.ID_Producto);
                });

            migrationBuilder.CreateTable(
                name: "DetalleAjusteProducto",
                columns: table => new
                {
                    ID_DetalleAjuste = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Ajuste = table.Column<int>(type: "int", nullable: false),
                    ID_Producto = table.Column<int>(type: "int", nullable: false),
                    CantidadAjustada = table.Column<int>(type: "int", nullable: false),
                    RazonAjuste = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleAjusteProducto", x => x.ID_DetalleAjuste);
                    table.ForeignKey(
                        name: "FK_DetalleAjusteProducto_AjusteProducto_ID_Ajuste",
                        column: x => x.ID_Ajuste,
                        principalTable: "AjusteProducto",
                        principalColumn: "ID_Ajuste",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetalleAjusteProducto_Producto_ID_Producto",
                        column: x => x.ID_Producto,
                        principalTable: "Producto",
                        principalColumn: "ID_Producto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovimientoInventario",
                columns: table => new
                {
                    ID_Movimiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Producto = table.Column<int>(type: "int", nullable: false),
                    TipoMovimiento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NumeroDocumento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FechaMovimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    StockMovimiento = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientoInventario", x => x.ID_Movimiento);
                    table.ForeignKey(
                        name: "FK_MovimientoInventario_Producto_ID_Producto",
                        column: x => x.ID_Producto,
                        principalTable: "Producto",
                        principalColumn: "ID_Producto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetalleAjusteProducto_ID_Ajuste",
                table: "DetalleAjusteProducto",
                column: "ID_Ajuste");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleAjusteProducto_ID_Producto",
                table: "DetalleAjusteProducto",
                column: "ID_Producto");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoInventario_ID_Producto",
                table: "MovimientoInventario",
                column: "ID_Producto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetalleAjusteProducto");

            migrationBuilder.DropTable(
                name: "MovimientoInventario");

            migrationBuilder.DropTable(
                name: "AjusteProducto");

            migrationBuilder.DropTable(
                name: "Producto");
        }
    }
}
