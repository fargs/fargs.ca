using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fargs.Portal.Migrations
{
    public partial class QuickbooksIntegration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Aginzo");

            migrationBuilder.EnsureSchema(
                name: "ServiceConnection");

            migrationBuilder.CreateTable(
                name: "HarvestExport",
                schema: "Aginzo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    Client = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Project = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ItemUnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ItemAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ItemDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ItemTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ItemTax2 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HarvestExport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuickbooksConnection",
                schema: "ServiceConnection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RealmId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccessToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Scopes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuickbooksConnection", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HarvestExport",
                schema: "Aginzo");

            migrationBuilder.DropTable(
                name: "QuickbooksConnection",
                schema: "ServiceConnection");
        }
    }
}
