using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompareHare.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddTwoMoreUtilityId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LoaderDataIdentifier2",
                table: "StateUtilityIndices",
                type: "varchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "LoaderDataIdentifier3",
                table: "StateUtilityIndices",
                type: "varchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoaderDataIdentifier2",
                table: "StateUtilityIndices");

            migrationBuilder.DropColumn(
                name: "LoaderDataIdentifier3",
                table: "StateUtilityIndices");
        }
    }
}
