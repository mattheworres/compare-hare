using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompareHare.Api.Migrations
{
    /// <inheritdoc />
    public partial class MakeSUIHashNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StateUtilityIndexHash",
                table: "Alerts",
                type: "varchar(40)",
                maxLength: 40,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(40)",
                oldMaxLength: 40)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Alerts",
                keyColumn: "StateUtilityIndexHash",
                keyValue: null,
                column: "StateUtilityIndexHash",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "StateUtilityIndexHash",
                table: "Alerts",
                type: "varchar(40)",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(40)",
                oldMaxLength: 40,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
