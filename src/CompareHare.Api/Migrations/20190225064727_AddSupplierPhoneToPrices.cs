using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CompareHare.Api.Migrations
{
    public partial class AddSupplierPhoneToPrices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "TermEndDate",
                table: "UtilityPrices",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<bool>(
                name: "HasTermEndDate",
                table: "UtilityPrices",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SupplierPhone",
                table: "UtilityPrices",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TermEndDate",
                table: "UtilityPriceHistories",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<bool>(
                name: "HasTermEndDate",
                table: "UtilityPriceHistories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SupplierPhone",
                table: "UtilityPriceHistories",
                maxLength: 40,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasTermEndDate",
                table: "UtilityPrices");

            migrationBuilder.DropColumn(
                name: "SupplierPhone",
                table: "UtilityPrices");

            migrationBuilder.DropColumn(
                name: "HasTermEndDate",
                table: "UtilityPriceHistories");

            migrationBuilder.DropColumn(
                name: "SupplierPhone",
                table: "UtilityPriceHistories");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TermEndDate",
                table: "UtilityPrices",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TermEndDate",
                table: "UtilityPriceHistories",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
