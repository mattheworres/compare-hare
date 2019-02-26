using Microsoft.EntityFrameworkCore.Migrations;

namespace CompareHare.Api.Migrations
{
    public partial class AddNullableRenewable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "RenewablePercentage",
                table: "UtilityPrices",
                nullable: true,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<float>(
                name: "RenewablePercentage",
                table: "UtilityPriceHistories",
                nullable: true,
                oldClrType: typeof(float));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "RenewablePercentage",
                table: "UtilityPrices",
                nullable: false,
                oldClrType: typeof(float),
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "RenewablePercentage",
                table: "UtilityPriceHistories",
                nullable: false,
                oldClrType: typeof(float),
                oldNullable: true);
        }
    }
}
