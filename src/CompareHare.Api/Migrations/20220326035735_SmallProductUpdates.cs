using Microsoft.EntityFrameworkCore.Migrations;

namespace CompareHare.Api.Migrations
{
    public partial class SmallProductUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "TrackedProducts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Footnote",
                table: "ProductRetailerPriceHistories",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasScrapingFootnote",
                table: "ProductRetailerPriceHistories",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "TrackedProducts");

            migrationBuilder.DropColumn(
                name: "Footnote",
                table: "ProductRetailerPriceHistories");

            migrationBuilder.DropColumn(
                name: "HasScrapingFootnote",
                table: "ProductRetailerPriceHistories");
        }
    }
}
