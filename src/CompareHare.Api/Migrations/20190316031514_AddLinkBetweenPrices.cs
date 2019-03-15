using Microsoft.EntityFrameworkCore.Migrations;

namespace CompareHare.Api.Migrations
{
    public partial class AddLinkBetweenPrices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alert_StateUtilityIndices_StateUtilityIndexId",
                table: "Alert");

            migrationBuilder.DropForeignKey(
                name: "FK_Alert_AspNetUsers_UserId",
                table: "Alert");

            migrationBuilder.DropForeignKey(
                name: "FK_AlertMatches_Alert_AlertId",
                table: "AlertMatches");

            migrationBuilder.DropTable(
                name: "AlertUtilityPriceHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Alert",
                table: "Alert");

            migrationBuilder.RenameTable(
                name: "Alert",
                newName: "Alerts");

            migrationBuilder.RenameIndex(
                name: "IX_Alert_UserId",
                table: "Alerts",
                newName: "IX_Alerts_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Alert_StateUtilityIndexId",
                table: "Alerts",
                newName: "IX_Alerts_StateUtilityIndexId");

            migrationBuilder.AddColumn<int>(
                name: "UtilityPriceHistoryId",
                table: "UtilityPrices",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Alerts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Alerts",
                table: "Alerts",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AlertMatchUtilityPriceHistory",
                columns: table => new
                {
                    AlertMatchId = table.Column<int>(nullable: false),
                    UtilityPriceHistoryId = table.Column<int>(nullable: false),
                    Order = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertMatchUtilityPriceHistory", x => new { x.AlertMatchId, x.UtilityPriceHistoryId });
                    table.ForeignKey(
                        name: "FK_AlertMatchUtilityPriceHistory_AlertMatches_AlertMatchId",
                        column: x => x.AlertMatchId,
                        principalTable: "AlertMatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlertMatchUtilityPriceHistory_UtilityPriceHistories_UtilityP~",
                        column: x => x.UtilityPriceHistoryId,
                        principalTable: "UtilityPriceHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UtilityPrices_UtilityPriceHistoryId",
                table: "UtilityPrices",
                column: "UtilityPriceHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AlertMatchUtilityPriceHistory_UtilityPriceHistoryId",
                table: "AlertMatchUtilityPriceHistory",
                column: "UtilityPriceHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AlertMatches_Alerts_AlertId",
                table: "AlertMatches",
                column: "AlertId",
                principalTable: "Alerts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Alerts_StateUtilityIndices_StateUtilityIndexId",
                table: "Alerts",
                column: "StateUtilityIndexId",
                principalTable: "StateUtilityIndices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Alerts_AspNetUsers_UserId",
                table: "Alerts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UtilityPrices_UtilityPriceHistories_UtilityPriceHistoryId",
                table: "UtilityPrices",
                column: "UtilityPriceHistoryId",
                principalTable: "UtilityPriceHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlertMatches_Alerts_AlertId",
                table: "AlertMatches");

            migrationBuilder.DropForeignKey(
                name: "FK_Alerts_StateUtilityIndices_StateUtilityIndexId",
                table: "Alerts");

            migrationBuilder.DropForeignKey(
                name: "FK_Alerts_AspNetUsers_UserId",
                table: "Alerts");

            migrationBuilder.DropForeignKey(
                name: "FK_UtilityPrices_UtilityPriceHistories_UtilityPriceHistoryId",
                table: "UtilityPrices");

            migrationBuilder.DropTable(
                name: "AlertMatchUtilityPriceHistory");

            migrationBuilder.DropIndex(
                name: "IX_UtilityPrices_UtilityPriceHistoryId",
                table: "UtilityPrices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Alerts",
                table: "Alerts");

            migrationBuilder.DropColumn(
                name: "UtilityPriceHistoryId",
                table: "UtilityPrices");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Alerts");

            migrationBuilder.RenameTable(
                name: "Alerts",
                newName: "Alert");

            migrationBuilder.RenameIndex(
                name: "IX_Alerts_UserId",
                table: "Alert",
                newName: "IX_Alert_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Alerts_StateUtilityIndexId",
                table: "Alert",
                newName: "IX_Alert_StateUtilityIndexId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Alert",
                table: "Alert",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AlertUtilityPriceHistory",
                columns: table => new
                {
                    AlertMatchId = table.Column<int>(nullable: false),
                    UtilityPriceHistoryId = table.Column<int>(nullable: false),
                    Order = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertUtilityPriceHistory", x => new { x.AlertMatchId, x.UtilityPriceHistoryId });
                    table.ForeignKey(
                        name: "FK_AlertUtilityPriceHistory_AlertMatches_AlertMatchId",
                        column: x => x.AlertMatchId,
                        principalTable: "AlertMatches",
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

            migrationBuilder.AddForeignKey(
                name: "FK_Alert_StateUtilityIndices_StateUtilityIndexId",
                table: "Alert",
                column: "StateUtilityIndexId",
                principalTable: "StateUtilityIndices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Alert_AspNetUsers_UserId",
                table: "Alert",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AlertMatches_Alert_AlertId",
                table: "AlertMatches",
                column: "AlertId",
                principalTable: "Alert",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
