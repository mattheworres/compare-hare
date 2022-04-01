using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CompareHare.Api.Migrations
{
    public partial class AddPriceTableAndChangeFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "AmountChange",
                table: "ProductRetailerPriceHistories",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "PercentChange",
                table: "ProductRetailerPriceHistories",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductRetailerPrices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TrackedProductId = table.Column<int>(nullable: false),
                    ProductRetailerPriceHistoryId = table.Column<int>(nullable: false),
                    ProductRetailer = table.Column<int>(nullable: false),
                    Price = table.Column<float>(nullable: true),
                    AmountChange = table.Column<float>(nullable: true),
                    PercentChange = table.Column<float>(nullable: true),
                    HasScrapingFootnote = table.Column<bool>(nullable: false),
                    Footnote = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRetailerPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductRetailerPrices_ProductRetailerPriceHistories_ProductR~",
                        column: x => x.ProductRetailerPriceHistoryId,
                        principalTable: "ProductRetailerPriceHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductRetailerPrices_TrackedProducts_TrackedProductId",
                        column: x => x.TrackedProductId,
                        principalTable: "TrackedProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductRetailerPrices_ProductRetailerPriceHistoryId",
                table: "ProductRetailerPrices",
                column: "ProductRetailerPriceHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRetailerPrices_TrackedProductId",
                table: "ProductRetailerPrices",
                column: "TrackedProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductRetailerPrices");

            migrationBuilder.DropColumn(
                name: "AmountChange",
                table: "ProductRetailerPriceHistories");

            migrationBuilder.DropColumn(
                name: "PercentChange",
                table: "ProductRetailerPriceHistories");
        }
    }
}
