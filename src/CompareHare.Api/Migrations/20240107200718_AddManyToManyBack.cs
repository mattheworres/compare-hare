using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompareHare.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddManyToManyBack : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AlertMatchUtilityPriceHistories",
                columns: table => new
                {
                    AlertMatchId = table.Column<int>(type: "int", nullable: false),
                    UtilityPriceHistoryId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<byte>(type: "tinyint unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertMatchUtilityPriceHistories", x => new { x.AlertMatchId, x.UtilityPriceHistoryId });
                    table.ForeignKey(
                        name: "FK_AlertMatchUtilityPriceHistories_AlertMatches_AlertMatchId",
                        column: x => x.AlertMatchId,
                        principalTable: "AlertMatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlertMatchUtilityPriceHistories_UtilityPriceHistories_Utilit~",
                        column: x => x.UtilityPriceHistoryId,
                        principalTable: "UtilityPriceHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AlertMatchUtilityPriceHistories_UtilityPriceHistoryId",
                table: "AlertMatchUtilityPriceHistories",
                column: "UtilityPriceHistoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlertMatchUtilityPriceHistories");
        }
    }
}
