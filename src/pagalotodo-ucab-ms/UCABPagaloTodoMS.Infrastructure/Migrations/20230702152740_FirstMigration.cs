using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UCABPagaloTodoMS.Infrastructure.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Administrador",
                columns: table => new
                {
                    Username = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocIdentidad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoVj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Estatus = table.Column<bool>(type: "bit", nullable: false),
                    TokenSeg = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrador", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "Consumidor",
                columns: table => new
                {
                    Username = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocIdentidad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoVj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Estatus = table.Column<bool>(type: "bit", nullable: false),
                    TokenSeg = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consumidor", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "Formato",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NombreCampo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoDato = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formato", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prestador",
                columns: table => new
                {
                    Username = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocIdentidad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoVj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Estatus = table.Column<bool>(type: "bit", nullable: false),
                    TokenSeg = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prestador", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "Servicio",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estatus = table.Column<int>(type: "int", nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoPago = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrestadorEntityId = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    FormatoConEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Servicio_Formato_FormatoConEntityId",
                        column: x => x.FormatoConEntityId,
                        principalTable: "Formato",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Servicio_Prestador_PrestadorEntityId",
                        column: x => x.PrestadorEntityId,
                        principalTable: "Prestador",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Deuda",
                columns: table => new
                {
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    servicioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Monto = table.Column<double>(type: "float", nullable: false),
                    Estatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deuda", x => new { x.Username, x.servicioId });
                    table.ForeignKey(
                        name: "FK_Deuda_Servicio_servicioId",
                        column: x => x.servicioId,
                        principalTable: "Servicio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormatoServicio",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServicioEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormatoConEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Requerido = table.Column<bool>(type: "bit", nullable: false),
                    Logitud = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormatoServicio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormatoServicio_Formato_FormatoConEntityId",
                        column: x => x.FormatoConEntityId,
                        principalTable: "Formato",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormatoServicio_Servicio_ServicioEntityId",
                        column: x => x.ServicioEntityId,
                        principalTable: "Servicio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pago",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Monto = table.Column<double>(type: "float", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Aprobado = table.Column<bool>(type: "bit", nullable: false),
                    Cierre = table.Column<bool>(type: "bit", nullable: false),
                    ServicioEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConsumidorEntityId = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    FechaCierre = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pago", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pago_Consumidor_ConsumidorEntityId",
                        column: x => x.ConsumidorEntityId,
                        principalTable: "Consumidor",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pago_Servicio_ServicioEntityId",
                        column: x => x.ServicioEntityId,
                        principalTable: "Servicio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deuda_servicioId",
                table: "Deuda",
                column: "servicioId");

            migrationBuilder.CreateIndex(
                name: "IX_FormatoServicio_FormatoConEntityId",
                table: "FormatoServicio",
                column: "FormatoConEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_FormatoServicio_ServicioEntityId",
                table: "FormatoServicio",
                column: "ServicioEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Pago_ConsumidorEntityId",
                table: "Pago",
                column: "ConsumidorEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Pago_ServicioEntityId",
                table: "Pago",
                column: "ServicioEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Servicio_FormatoConEntityId",
                table: "Servicio",
                column: "FormatoConEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Servicio_PrestadorEntityId",
                table: "Servicio",
                column: "PrestadorEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Administrador");

            migrationBuilder.DropTable(
                name: "Deuda");

            migrationBuilder.DropTable(
                name: "FormatoServicio");

            migrationBuilder.DropTable(
                name: "Pago");

            migrationBuilder.DropTable(
                name: "Consumidor");

            migrationBuilder.DropTable(
                name: "Servicio");

            migrationBuilder.DropTable(
                name: "Formato");

            migrationBuilder.DropTable(
                name: "Prestador");
        }
    }
}
