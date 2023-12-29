using Microsoft.EntityFrameworkCore.Migrations;

namespace CompareHare.Api.Migrations
{
    public partial class RemovePriceSelectorRequirement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PriceSelector",
                table: "TrackedProductRetailers",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 128);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PriceSelector",
                table: "TrackedProductRetailers",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128,
                oldNullable: true);
        }
    }
}
