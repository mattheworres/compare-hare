using Microsoft.EntityFrameworkCore.Migrations;

namespace CompareHare.Api.Migrations
{
    public partial class AddLinkToProdRetailersPt2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProductRetailerPrices_TrackedProductRetailerId",
                table: "ProductRetailerPrices",
                column: "TrackedProductRetailerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRetailerPriceHistories_TrackedProductRetailerId",
                table: "ProductRetailerPriceHistories",
                column: "TrackedProductRetailerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRetailerPriceHistories_TrackedProductRetailers_Tracke~",
                table: "ProductRetailerPriceHistories",
                column: "TrackedProductRetailerId",
                principalTable: "TrackedProductRetailers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRetailerPrices_TrackedProductRetailers_TrackedProduct~",
                table: "ProductRetailerPrices",
                column: "TrackedProductRetailerId",
                principalTable: "TrackedProductRetailers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductRetailerPriceHistories_TrackedProductRetailers_Tracke~",
                table: "ProductRetailerPriceHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductRetailerPrices_TrackedProductRetailers_TrackedProduct~",
                table: "ProductRetailerPrices");

            migrationBuilder.DropIndex(
                name: "IX_ProductRetailerPrices_TrackedProductRetailerId",
                table: "ProductRetailerPrices");

            migrationBuilder.DropIndex(
                name: "IX_ProductRetailerPriceHistories_TrackedProductRetailerId",
                table: "ProductRetailerPriceHistories");
        }
    }
}
