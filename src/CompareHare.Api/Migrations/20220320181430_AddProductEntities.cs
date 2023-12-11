using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CompareHare.Api.Migrations
{
    public partial class AddProductEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrackedProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackedProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackedProducts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductRetailerPriceHistories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TrackedProductId = table.Column<int>(nullable: false),
                    ProductRetailer = table.Column<int>(nullable: false),
                    Price = table.Column<float>(nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRetailerPriceHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductRetailerPriceHistories_TrackedProducts_TrackedProduct~",
                        column: x => x.TrackedProductId,
                        principalTable: "TrackedProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrackedProductRetailers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TrackedProductId = table.Column<int>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false),
                    ProductRetailer = table.Column<int>(nullable: false),
                    ScrapeUrl = table.Column<string>(maxLength: 512, nullable: false),
                    PriceSelector = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackedProductRetailers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackedProductRetailers_TrackedProducts_TrackedProductId",
                        column: x => x.TrackedProductId,
                        principalTable: "TrackedProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductRetailerPriceHistories_TrackedProductId",
                table: "ProductRetailerPriceHistories",
                column: "TrackedProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedProductRetailers_TrackedProductId",
                table: "TrackedProductRetailers",
                column: "TrackedProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedProducts_UserId",
                table: "TrackedProducts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedProductRetailers_ProductRetailer",
                table: "TrackedProductRetailers",
                column: "ProductRetailer");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRetailerPriceHistories_ProductRetailer",
                table: "ProductRetailerPriceHistories",
                column: "ProductRetailer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductRetailerPriceHistories");

            migrationBuilder.DropTable(
                name: "TrackedProductRetailers");

            migrationBuilder.DropTable(
                name: "TrackedProducts");
        }
    }
}
