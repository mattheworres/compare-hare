using Microsoft.EntityFrameworkCore.Migrations;

namespace CompareHare.Api.Migrations
{
    public partial class AddOfferIdToPrices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OfferId",
                table: "UtilityPrices",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OfferId",
                table: "UtilityPriceHistories",
                maxLength: 64,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OfferId",
                table: "UtilityPrices");

            migrationBuilder.DropColumn(
                name: "OfferId",
                table: "UtilityPriceHistories");
        }
    }
}
