using Microsoft.EntityFrameworkCore.Migrations;

namespace CompareHare.Api.Migrations
{
    public partial class AddNullablePricePerUnit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "PricePerUnit",
                table: "UtilityPrices",
                nullable: true,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<float>(
                name: "PricePerUnit",
                table: "UtilityPriceHistories",
                nullable: true,
                oldClrType: typeof(float));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "PricePerUnit",
                table: "UtilityPrices",
                nullable: false,
                oldClrType: typeof(float),
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "PricePerUnit",
                table: "UtilityPriceHistories",
                nullable: false,
                oldClrType: typeof(float),
                oldNullable: true);
        }
    }
}
