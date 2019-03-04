using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CompareHare.Api.Migrations
{
    public partial class RenameAlertCriteriaToAlert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlertMatches_AlertCriteria_AlertCriteriaId",
                table: "AlertMatches");

            migrationBuilder.DropTable(
                name: "AlertCriteria");

            migrationBuilder.RenameColumn(
                name: "AlertCriteriaId",
                table: "AlertMatches",
                newName: "AlertId");

            migrationBuilder.RenameIndex(
                name: "IX_AlertMatches_AlertCriteriaId",
                table: "AlertMatches",
                newName: "IX_AlertMatches_AlertId");

            migrationBuilder.CreateTable(
                name: "Alert",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    StateUtilityIndexId = table.Column<int>(nullable: false),
                    StateUtilityIndexHash = table.Column<string>(maxLength: 40, nullable: true),
                    MinimumPrice = table.Column<decimal>(nullable: false),
                    MaximumPrice = table.Column<decimal>(nullable: false),
                    HasRenewable = table.Column<bool>(nullable: false),
                    MinimumRenewablePercent = table.Column<decimal>(nullable: false),
                    MaximumRenewablePercent = table.Column<decimal>(nullable: false),
                    MinimumMonthLength = table.Column<int>(nullable: false),
                    MaximumMonthLength = table.Column<int>(nullable: false),
                    HasCancellationFee = table.Column<bool>(nullable: false),
                    HasMonthlyFee = table.Column<bool>(nullable: false),
                    HasEnrollmentFee = table.Column<bool>(nullable: false),
                    HasNetMetering = table.Column<bool>(nullable: false),
                    RequiresDeposit = table.Column<bool>(nullable: false),
                    HasBulkDiscounts = table.Column<bool>(nullable: false),
                    Comments = table.Column<string>(maxLength: 512, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alert", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alert_StateUtilityIndices_StateUtilityIndexId",
                        column: x => x.StateUtilityIndexId,
                        principalTable: "StateUtilityIndices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alert_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alert_StateUtilityIndexId",
                table: "Alert",
                column: "StateUtilityIndexId");

            migrationBuilder.CreateIndex(
                name: "IX_Alert_UserId",
                table: "Alert",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AlertMatches_Alert_AlertId",
                table: "AlertMatches",
                column: "AlertId",
                principalTable: "Alert",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlertMatches_Alert_AlertId",
                table: "AlertMatches");

            migrationBuilder.DropTable(
                name: "Alert");

            migrationBuilder.RenameColumn(
                name: "AlertId",
                table: "AlertMatches",
                newName: "AlertCriteriaId");

            migrationBuilder.RenameIndex(
                name: "IX_AlertMatches_AlertId",
                table: "AlertMatches",
                newName: "IX_AlertMatches_AlertCriteriaId");

            migrationBuilder.CreateTable(
                name: "AlertCriteria",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Comments = table.Column<string>(maxLength: 512, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    HasBulkDiscounts = table.Column<bool>(nullable: false),
                    HasCancellationFee = table.Column<bool>(nullable: false),
                    HasEnrollmentFee = table.Column<bool>(nullable: false),
                    HasMonthlyFee = table.Column<bool>(nullable: false),
                    HasNetMetering = table.Column<bool>(nullable: false),
                    HasRenewable = table.Column<bool>(nullable: false),
                    MaximumMonthLength = table.Column<int>(nullable: false),
                    MaximumPrice = table.Column<decimal>(nullable: false),
                    MaximumRenewablePercent = table.Column<decimal>(nullable: false),
                    MinimumMonthLength = table.Column<int>(nullable: false),
                    MinimumPrice = table.Column<decimal>(nullable: false),
                    MinimumRenewablePercent = table.Column<decimal>(nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    RequiresDeposit = table.Column<bool>(nullable: false),
                    StateUtilityIndexHash = table.Column<string>(maxLength: 40, nullable: true),
                    StateUtilityIndexId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertCriteria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlertCriteria_StateUtilityIndices_StateUtilityIndexId",
                        column: x => x.StateUtilityIndexId,
                        principalTable: "StateUtilityIndices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlertCriteria_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlertCriteria_StateUtilityIndexId",
                table: "AlertCriteria",
                column: "StateUtilityIndexId");

            migrationBuilder.CreateIndex(
                name: "IX_AlertCriteria_UserId",
                table: "AlertCriteria",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AlertMatches_AlertCriteria_AlertCriteriaId",
                table: "AlertMatches",
                column: "AlertCriteriaId",
                principalTable: "AlertCriteria",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
