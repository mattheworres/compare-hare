using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CompareHare.Api.Migrations
{
    public partial class AddPriceScrapeExceptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "productPriceScrapingExceptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TrackedProductId = table.Column<int>(nullable: false),
                    TrackedProductRetailerId = table.Column<int>(nullable: false),
                    ProductRetailer = table.Column<int>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    Selector = table.Column<string>(nullable: true),
                    Error = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productPriceScrapingExceptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_productPriceScrapingExceptions_TrackedProducts_TrackedProduc~",
                        column: x => x.TrackedProductId,
                        principalTable: "TrackedProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_productPriceScrapingExceptions_TrackedProductRetailers_Track~",
                        column: x => x.TrackedProductRetailerId,
                        principalTable: "TrackedProductRetailers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_productPriceScrapingExceptions_TrackedProductId",
                table: "productPriceScrapingExceptions",
                column: "TrackedProductId");

            migrationBuilder.CreateIndex(
                name: "IX_productPriceScrapingExceptions_TrackedProductRetailerId",
                table: "productPriceScrapingExceptions",
                column: "TrackedProductRetailerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "productPriceScrapingExceptions");
        }
    }
}
