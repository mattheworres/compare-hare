using Microsoft.EntityFrameworkCore.Migrations;

namespace CompareHare.Api.Migrations
{
    public partial class AddAlertPriceHistoryLinker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AlertUtilityPriceHistory",
                columns: table => new
                {
                    AlertId = table.Column<int>(nullable: false),
                    UtilityPriceHistoryId = table.Column<int>(nullable: false),
                    Order = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertUtilityPriceHistory", x => new { x.AlertId, x.UtilityPriceHistoryId });
                    table.ForeignKey(
                        name: "FK_AlertUtilityPriceHistory_Alerts_AlertId",
                        column: x => x.AlertId,
                        principalTable: "Alerts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlertUtilityPriceHistory_UtilityPriceHistories_UtilityPriceH~",
                        column: x => x.UtilityPriceHistoryId,
                        principalTable: "UtilityPriceHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlertUtilityPriceHistory_UtilityPriceHistoryId",
                table: "AlertUtilityPriceHistory",
                column: "UtilityPriceHistoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlertUtilityPriceHistory");
        }
    }
}
