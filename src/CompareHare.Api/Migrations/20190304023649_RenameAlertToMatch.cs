using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CompareHare.Api.Migrations
{
    public partial class RenameAlertToMatch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlertUtilityPriceHistory_Alerts_AlertId",
                table: "AlertUtilityPriceHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_PendingAlertNotifications_Alerts_AlertId",
                table: "PendingAlertNotifications");

            migrationBuilder.DropTable(
                name: "Alerts");

            migrationBuilder.RenameColumn(
                name: "AlertId",
                table: "PendingAlertNotifications",
                newName: "AlertMatchId");

            migrationBuilder.RenameIndex(
                name: "IX_PendingAlertNotifications_AlertId",
                table: "PendingAlertNotifications",
                newName: "IX_PendingAlertNotifications_AlertMatchId");

            migrationBuilder.RenameColumn(
                name: "AlertId",
                table: "AlertUtilityPriceHistory",
                newName: "AlertMatchId");

            migrationBuilder.CreateTable(
                name: "AlertMatches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AlertCriteriaId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    StateUtilityIndexId = table.Column<int>(nullable: false),
                    AlertOfferHash = table.Column<string>(maxLength: 40, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertMatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlertMatches_AlertCriteria_AlertCriteriaId",
                        column: x => x.AlertCriteriaId,
                        principalTable: "AlertCriteria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlertMatches_StateUtilityIndices_StateUtilityIndexId",
                        column: x => x.StateUtilityIndexId,
                        principalTable: "StateUtilityIndices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlertMatches_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlertMatches_AlertCriteriaId",
                table: "AlertMatches",
                column: "AlertCriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_AlertMatches_StateUtilityIndexId",
                table: "AlertMatches",
                column: "StateUtilityIndexId");

            migrationBuilder.CreateIndex(
                name: "IX_AlertMatches_UserId",
                table: "AlertMatches",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AlertUtilityPriceHistory_AlertMatches_AlertMatchId",
                table: "AlertUtilityPriceHistory",
                column: "AlertMatchId",
                principalTable: "AlertMatches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PendingAlertNotifications_AlertMatches_AlertMatchId",
                table: "PendingAlertNotifications",
                column: "AlertMatchId",
                principalTable: "AlertMatches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlertUtilityPriceHistory_AlertMatches_AlertMatchId",
                table: "AlertUtilityPriceHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_PendingAlertNotifications_AlertMatches_AlertMatchId",
                table: "PendingAlertNotifications");

            migrationBuilder.DropTable(
                name: "AlertMatches");

            migrationBuilder.RenameColumn(
                name: "AlertMatchId",
                table: "PendingAlertNotifications",
                newName: "AlertId");

            migrationBuilder.RenameIndex(
                name: "IX_PendingAlertNotifications_AlertMatchId",
                table: "PendingAlertNotifications",
                newName: "IX_PendingAlertNotifications_AlertId");

            migrationBuilder.RenameColumn(
                name: "AlertMatchId",
                table: "AlertUtilityPriceHistory",
                newName: "AlertId");

            migrationBuilder.CreateTable(
                name: "Alerts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AlertCriteriaId = table.Column<int>(nullable: false),
                    AlertOfferHash = table.Column<string>(maxLength: 40, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    StateUtilityIndexId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alerts_AlertCriteria_AlertCriteriaId",
                        column: x => x.AlertCriteriaId,
                        principalTable: "AlertCriteria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alerts_StateUtilityIndices_StateUtilityIndexId",
                        column: x => x.StateUtilityIndexId,
                        principalTable: "StateUtilityIndices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alerts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_AlertCriteriaId",
                table: "Alerts",
                column: "AlertCriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_StateUtilityIndexId",
                table: "Alerts",
                column: "StateUtilityIndexId");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_UserId",
                table: "Alerts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AlertUtilityPriceHistory_Alerts_AlertId",
                table: "AlertUtilityPriceHistory",
                column: "AlertId",
                principalTable: "Alerts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PendingAlertNotifications_Alerts_AlertId",
                table: "PendingAlertNotifications",
                column: "AlertId",
                principalTable: "Alerts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
