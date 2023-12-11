using Microsoft.EntityFrameworkCore.Migrations;

namespace CompareHare.Api.Migrations
{
    public partial class AddLinkToProdRetailersPt1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrackedProductRetailerId",
                table: "ProductRetailerPrices",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TrackedProductRetailerId",
                table: "ProductRetailerPriceHistories",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrackedProductRetailerId",
                table: "ProductRetailerPrices");

            migrationBuilder.DropColumn(
                name: "TrackedProductRetailerId",
                table: "ProductRetailerPriceHistories");
        }
    }
}
