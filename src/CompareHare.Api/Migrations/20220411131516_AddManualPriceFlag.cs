using Microsoft.EntityFrameworkCore.Migrations;

namespace CompareHare.Api.Migrations
{
    public partial class AddManualPriceFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PriceIsManual",
                table: "ProductRetailerPrices",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PriceIsManual",
                table: "ProductRetailerPriceHistories",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceIsManual",
                table: "ProductRetailerPrices");

            migrationBuilder.DropColumn(
                name: "PriceIsManual",
                table: "ProductRetailerPriceHistories");
        }
    }
}
