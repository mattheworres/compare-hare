using Microsoft.EntityFrameworkCore.Migrations;

namespace CompareHare.Api.Migrations
{
    public partial class ProperCaseMakesWaste : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_productPriceScrapingExceptions_TrackedProducts_TrackedProduc~",
                table: "productPriceScrapingExceptions");

            migrationBuilder.DropForeignKey(
                name: "FK_productPriceScrapingExceptions_TrackedProductRetailers_Track~",
                table: "productPriceScrapingExceptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_productPriceScrapingExceptions",
                table: "productPriceScrapingExceptions");

            migrationBuilder.RenameTable(
                name: "productPriceScrapingExceptions",
                newName: "ProductPriceScrapingExceptions");

            migrationBuilder.RenameIndex(
                name: "IX_productPriceScrapingExceptions_TrackedProductRetailerId",
                table: "ProductPriceScrapingExceptions",
                newName: "IX_ProductPriceScrapingExceptions_TrackedProductRetailerId");

            migrationBuilder.RenameIndex(
                name: "IX_productPriceScrapingExceptions_TrackedProductId",
                table: "ProductPriceScrapingExceptions",
                newName: "IX_ProductPriceScrapingExceptions_TrackedProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductPriceScrapingExceptions",
                table: "ProductPriceScrapingExceptions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPriceScrapingExceptions_TrackedProducts_TrackedProduc~",
                table: "ProductPriceScrapingExceptions",
                column: "TrackedProductId",
                principalTable: "TrackedProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPriceScrapingExceptions_TrackedProductRetailers_Track~",
                table: "ProductPriceScrapingExceptions",
                column: "TrackedProductRetailerId",
                principalTable: "TrackedProductRetailers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPriceScrapingExceptions_TrackedProducts_TrackedProduc~",
                table: "ProductPriceScrapingExceptions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPriceScrapingExceptions_TrackedProductRetailers_Track~",
                table: "ProductPriceScrapingExceptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductPriceScrapingExceptions",
                table: "ProductPriceScrapingExceptions");

            migrationBuilder.RenameTable(
                name: "ProductPriceScrapingExceptions",
                newName: "productPriceScrapingExceptions");

            migrationBuilder.RenameIndex(
                name: "IX_ProductPriceScrapingExceptions_TrackedProductRetailerId",
                table: "productPriceScrapingExceptions",
                newName: "IX_productPriceScrapingExceptions_TrackedProductRetailerId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductPriceScrapingExceptions_TrackedProductId",
                table: "productPriceScrapingExceptions",
                newName: "IX_productPriceScrapingExceptions_TrackedProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_productPriceScrapingExceptions",
                table: "productPriceScrapingExceptions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_productPriceScrapingExceptions_TrackedProducts_TrackedProduc~",
                table: "productPriceScrapingExceptions",
                column: "TrackedProductId",
                principalTable: "TrackedProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_productPriceScrapingExceptions_TrackedProductRetailers_Track~",
                table: "productPriceScrapingExceptions",
                column: "TrackedProductRetailerId",
                principalTable: "TrackedProductRetailers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
