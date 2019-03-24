using Microsoft.EntityFrameworkCore.Migrations;

namespace CompareHare.Api.Migrations
{
    public partial class FixRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlertMatchUtilityPriceHistory_AlertMatches_AlertMatchId",
                table: "AlertMatchUtilityPriceHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_AlertMatchUtilityPriceHistory_UtilityPriceHistories_UtilityP~",
                table: "AlertMatchUtilityPriceHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AlertMatchUtilityPriceHistory",
                table: "AlertMatchUtilityPriceHistory");

            migrationBuilder.RenameTable(
                name: "AlertMatchUtilityPriceHistory",
                newName: "AlertMatchUtilityPriceHistories");

            migrationBuilder.RenameIndex(
                name: "IX_AlertMatchUtilityPriceHistory_UtilityPriceHistoryId",
                table: "AlertMatchUtilityPriceHistories",
                newName: "IX_AlertMatchUtilityPriceHistories_UtilityPriceHistoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AlertMatchUtilityPriceHistories",
                table: "AlertMatchUtilityPriceHistories",
                columns: new[] { "AlertMatchId", "UtilityPriceHistoryId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AlertMatchUtilityPriceHistories_AlertMatches_AlertMatchId",
                table: "AlertMatchUtilityPriceHistories",
                column: "AlertMatchId",
                principalTable: "AlertMatches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AlertMatchUtilityPriceHistories_UtilityPriceHistories_Utilit~",
                table: "AlertMatchUtilityPriceHistories",
                column: "UtilityPriceHistoryId",
                principalTable: "UtilityPriceHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlertMatchUtilityPriceHistories_AlertMatches_AlertMatchId",
                table: "AlertMatchUtilityPriceHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_AlertMatchUtilityPriceHistories_UtilityPriceHistories_Utilit~",
                table: "AlertMatchUtilityPriceHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AlertMatchUtilityPriceHistories",
                table: "AlertMatchUtilityPriceHistories");

            migrationBuilder.RenameTable(
                name: "AlertMatchUtilityPriceHistories",
                newName: "AlertMatchUtilityPriceHistory");

            migrationBuilder.RenameIndex(
                name: "IX_AlertMatchUtilityPriceHistories_UtilityPriceHistoryId",
                table: "AlertMatchUtilityPriceHistory",
                newName: "IX_AlertMatchUtilityPriceHistory_UtilityPriceHistoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AlertMatchUtilityPriceHistory",
                table: "AlertMatchUtilityPriceHistory",
                columns: new[] { "AlertMatchId", "UtilityPriceHistoryId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AlertMatchUtilityPriceHistory_AlertMatches_AlertMatchId",
                table: "AlertMatchUtilityPriceHistory",
                column: "AlertMatchId",
                principalTable: "AlertMatches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AlertMatchUtilityPriceHistory_UtilityPriceHistories_UtilityP~",
                table: "AlertMatchUtilityPriceHistory",
                column: "UtilityPriceHistoryId",
                principalTable: "UtilityPriceHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
