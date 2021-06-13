using Microsoft.EntityFrameworkCore.Migrations;

namespace Fargs.Portal.Migrations
{
    public partial class HarvestExport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ItemUnitPrice",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ItemTax2",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ItemTax",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ItemQuantity",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ItemDiscount",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ItemAmount",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "HarvestConsultant",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemType2",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PayAmount",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "QB_Account",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QB_BillDate",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QB_BillNo",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "QB_Description",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QB_Terms",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QB_Vendor",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SL1_Amount",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SL1_Percent",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "SL1_QB_Account",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SL1_QB_Vendor",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SL2_Amount",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SL2_Percent",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "SL2_QB_Account",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SL2_QB_Vendor",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SS1_Amount",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SS1_Percent",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "SS1_QB_Account",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SS1_QB_Vendor",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SS2_Amount",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SS2_Percent",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "SS2_QB_Account",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SS2_QB_Vendor",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SalesLead_1",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SalesLead_2",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SalesSupport_1",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SalesSupport_2",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HarvestConsultant",
                schema: "Aginzo",
                table: "HarvestExport");

            migrationBuilder.DropColumn(
                name: "ItemType2",
                schema: "Aginzo",
                table: "HarvestExport");

            migrationBuilder.DropColumn(
                name: "PayAmount",
                schema: "Aginzo",
                table: "HarvestExport");

            migrationBuilder.DropColumn(
                name: "QB_Account",
                schema: "Aginzo",
                table: "HarvestExport");

            migrationBuilder.DropColumn(
                name: "QB_BillDate",
                schema: "Aginzo",
                table: "HarvestExport");

            migrationBuilder.DropColumn(
                name: "QB_BillNo",
                schema: "Aginzo",
                table: "HarvestExport");

            migrationBuilder.DropColumn(
                name: "QB_Description",
                schema: "Aginzo",
                table: "HarvestExport");

            migrationBuilder.DropColumn(
                name: "QB_Terms",
                schema: "Aginzo",
                table: "HarvestExport");

            migrationBuilder.DropColumn(
                name: "QB_Vendor",
                schema: "Aginzo",
                table: "HarvestExport");

            migrationBuilder.DropColumn(
                name: "SL1_Amount",
                schema: "Aginzo",
                table: "HarvestExport");

            migrationBuilder.DropColumn(
                name: "SL1_Percent",
                schema: "Aginzo",
                table: "HarvestExport");

            migrationBuilder.DropColumn(
                name: "SL1_QB_Account",
                schema: "Aginzo",
                table: "HarvestExport");

            migrationBuilder.DropColumn(
                name: "SL1_QB_Vendor",
                schema: "Aginzo",
                table: "HarvestExport");

            migrationBuilder.DropColumn(
                name: "SL2_Amount",
                schema: "Aginzo",
                table: "HarvestExport");

            migrationBuilder.DropColumn(
                name: "SL2_Percent",
                schema: "Aginzo",
                table: "HarvestExport");

            migrationBuilder.DropColumn(
                name: "SL2_QB_Account",
                schema: "Aginzo",
                table: "HarvestExport");

            migrationBuilder.DropColumn(
                name: "SL2_QB_Vendor",
                schema: "Aginzo",
                table: "HarvestExport");

            migrationBuilder.DropColumn(
                name: "SS1_Amount",
                schema: "Aginzo",
                table: "HarvestExport");

            migrationBuilder.DropColumn(
                name: "SS1_Percent",
                schema: "Aginzo",
                table: "HarvestExport");

            migrationBuilder.DropColumn(
                name: "SS1_QB_Account",
                schema: "Aginzo",
                table: "HarvestExport");

            migrationBuilder.DropColumn(
                name: "SS1_QB_Vendor",
                schema: "Aginzo",
                table: "HarvestExport");

            migrationBuilder.DropColumn(
                name: "SS2_Amount",
                schema: "Aginzo",
                table: "HarvestExport");

            migrationBuilder.DropColumn(
                name: "SS2_Percent",
                schema: "Aginzo",
                table: "HarvestExport");

            migrationBuilder.DropColumn(
                name: "SS2_QB_Account",
                schema: "Aginzo",
                table: "HarvestExport");

            migrationBuilder.DropColumn(
                name: "SS2_QB_Vendor",
                schema: "Aginzo",
                table: "HarvestExport");

            migrationBuilder.DropColumn(
                name: "SalesLead_1",
                schema: "Aginzo",
                table: "HarvestExport");

            migrationBuilder.DropColumn(
                name: "SalesLead_2",
                schema: "Aginzo",
                table: "HarvestExport");

            migrationBuilder.DropColumn(
                name: "SalesSupport_1",
                schema: "Aginzo",
                table: "HarvestExport");

            migrationBuilder.DropColumn(
                name: "SalesSupport_2",
                schema: "Aginzo",
                table: "HarvestExport");

            migrationBuilder.AlterColumn<decimal>(
                name: "ItemUnitPrice",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "ItemTax2",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "ItemTax",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "ItemQuantity",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "ItemDiscount",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "ItemAmount",
                schema: "Aginzo",
                table: "HarvestExport",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);
        }
    }
}
