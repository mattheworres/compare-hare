using Microsoft.EntityFrameworkCore.Migrations;

namespace CompareHare.Api.Migrations
{
    public partial class MakePriceDataNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TermMonthLength",
                table: "UtilityPrices",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<bool>(
                name: "RequiresDeposit",
                table: "UtilityPrices",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "IsIntroductoryPrice",
                table: "UtilityPrices",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "HasRenewable",
                table: "UtilityPrices",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "HasNetMetering",
                table: "UtilityPrices",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "HasMonthlyFee",
                table: "UtilityPrices",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "HasEnrollmentFee",
                table: "UtilityPrices",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "HasCancellationFee",
                table: "UtilityPrices",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "HasBulkDiscounts",
                table: "UtilityPrices",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<int>(
                name: "TermMonthLength",
                table: "UtilityPriceHistories",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<bool>(
                name: "RequiresDeposit",
                table: "UtilityPriceHistories",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "IsIntroductoryPrice",
                table: "UtilityPriceHistories",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "HasTermEndDate",
                table: "UtilityPriceHistories",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "HasRenewable",
                table: "UtilityPriceHistories",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "HasNetMetering",
                table: "UtilityPriceHistories",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "HasMonthlyFee",
                table: "UtilityPriceHistories",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "HasEnrollmentFee",
                table: "UtilityPriceHistories",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "HasCancellationFee",
                table: "UtilityPriceHistories",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "HasBulkDiscounts",
                table: "UtilityPriceHistories",
                nullable: true,
                oldClrType: typeof(bool));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TermMonthLength",
                table: "UtilityPrices",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "RequiresDeposit",
                table: "UtilityPrices",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsIntroductoryPrice",
                table: "UtilityPrices",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "HasRenewable",
                table: "UtilityPrices",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "HasNetMetering",
                table: "UtilityPrices",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "HasMonthlyFee",
                table: "UtilityPrices",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "HasEnrollmentFee",
                table: "UtilityPrices",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "HasCancellationFee",
                table: "UtilityPrices",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "HasBulkDiscounts",
                table: "UtilityPrices",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TermMonthLength",
                table: "UtilityPriceHistories",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "RequiresDeposit",
                table: "UtilityPriceHistories",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsIntroductoryPrice",
                table: "UtilityPriceHistories",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "HasTermEndDate",
                table: "UtilityPriceHistories",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "HasRenewable",
                table: "UtilityPriceHistories",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "HasNetMetering",
                table: "UtilityPriceHistories",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "HasMonthlyFee",
                table: "UtilityPriceHistories",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "HasEnrollmentFee",
                table: "UtilityPriceHistories",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "HasCancellationFee",
                table: "UtilityPriceHistories",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "HasBulkDiscounts",
                table: "UtilityPriceHistories",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }
    }
}
