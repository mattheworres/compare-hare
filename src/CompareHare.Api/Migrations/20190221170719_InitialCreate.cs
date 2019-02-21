using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CompareHare.Api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 255, nullable: true),
                    LastName = table.Column<string>(maxLength: 255, nullable: true),
                    TimeZone = table.Column<string>(maxLength: 45, nullable: true),
                    PasswordResetToken = table.Column<string>(maxLength: 128, nullable: true),
                    PasswordResetTokenExpirationDate = table.Column<DateTimeOffset>(nullable: true),
                    FirstLogin = table.Column<DateTime>(nullable: true),
                    LastLogin = table.Column<DateTime>(nullable: true),
                    FullAccessGrantedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StateUtilityIndices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LoaderDataIdentifier = table.Column<string>(maxLength: 256, nullable: true),
                    UtilityState = table.Column<int>(nullable: false),
                    UtilityType = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    LastUpdatedHash = table.Column<string>(maxLength: 40, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateUtilityIndices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlertCriteria",
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

            migrationBuilder.CreateTable(
                name: "UtilityPriceHistories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StateUtilityIndexId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    PriceUnit = table.Column<string>(maxLength: 50, nullable: true),
                    PricePerUnit = table.Column<float>(nullable: false),
                    FlatRate = table.Column<string>(maxLength: 120, nullable: true),
                    PriceStructure = table.Column<string>(maxLength: 255, nullable: true),
                    HasCancellationFee = table.Column<bool>(nullable: false),
                    CancellationFee = table.Column<string>(nullable: true),
                    HasMonthlyFee = table.Column<bool>(nullable: false),
                    MonthlyFee = table.Column<string>(nullable: true),
                    HasEnrollmentFee = table.Column<bool>(nullable: false),
                    EnrollmentFee = table.Column<string>(nullable: true),
                    HasNetMetering = table.Column<bool>(nullable: false),
                    NetMetering = table.Column<string>(nullable: true),
                    RequiresDeposit = table.Column<bool>(nullable: false),
                    HasBulkDiscounts = table.Column<bool>(nullable: false),
                    IsIntroductoryPrice = table.Column<bool>(nullable: false),
                    HasRenewable = table.Column<bool>(nullable: false),
                    RenewablePercentage = table.Column<float>(nullable: false),
                    TermMonthLength = table.Column<int>(nullable: false),
                    TermEndDate = table.Column<DateTime>(nullable: false),
                    OfferUrl = table.Column<string>(maxLength: 256, nullable: true),
                    Comments = table.Column<string>(maxLength: 512, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UtilityPriceHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UtilityPriceHistories_StateUtilityIndices_StateUtilityIndexId",
                        column: x => x.StateUtilityIndexId,
                        principalTable: "StateUtilityIndices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UtilityPrices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StateUtilityIndexId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    PriceUnit = table.Column<string>(maxLength: 50, nullable: true),
                    PricePerUnit = table.Column<float>(nullable: false),
                    FlatRate = table.Column<string>(maxLength: 120, nullable: true),
                    PriceStructure = table.Column<string>(maxLength: 255, nullable: true),
                    HasCancellationFee = table.Column<bool>(nullable: false),
                    CancellationFee = table.Column<string>(nullable: true),
                    HasMonthlyFee = table.Column<bool>(nullable: false),
                    MonthlyFee = table.Column<string>(nullable: true),
                    HasEnrollmentFee = table.Column<bool>(nullable: false),
                    EnrollmentFee = table.Column<string>(nullable: true),
                    HasNetMetering = table.Column<bool>(nullable: false),
                    NetMetering = table.Column<string>(nullable: true),
                    RequiresDeposit = table.Column<bool>(nullable: false),
                    HasBulkDiscounts = table.Column<bool>(nullable: false),
                    IsIntroductoryPrice = table.Column<bool>(nullable: false),
                    HasRenewable = table.Column<bool>(nullable: false),
                    RenewablePercentage = table.Column<float>(nullable: false),
                    TermMonthLength = table.Column<int>(nullable: false),
                    TermEndDate = table.Column<DateTime>(nullable: false),
                    OfferUrl = table.Column<string>(maxLength: 256, nullable: true),
                    Comments = table.Column<string>(maxLength: 512, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UtilityPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UtilityPrices_StateUtilityIndices_StateUtilityIndexId",
                        column: x => x.StateUtilityIndexId,
                        principalTable: "StateUtilityIndices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alerts",
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

            migrationBuilder.CreateTable(
                name: "PendingAlertNotifications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    AlertId = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingAlertNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PendingAlertNotifications_Alerts_AlertId",
                        column: x => x.AlertId,
                        principalTable: "Alerts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PendingAlertNotifications_AspNetUsers_UserId",
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

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_UserId",
                table: "AspNetRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PendingAlertNotifications_AlertId",
                table: "PendingAlertNotifications",
                column: "AlertId");

            migrationBuilder.CreateIndex(
                name: "IX_PendingAlertNotifications_UserId",
                table: "PendingAlertNotifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UtilityPriceHistories_StateUtilityIndexId",
                table: "UtilityPriceHistories",
                column: "StateUtilityIndexId");

            migrationBuilder.CreateIndex(
                name: "IX_UtilityPrices_StateUtilityIndexId",
                table: "UtilityPrices",
                column: "StateUtilityIndexId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "PendingAlertNotifications");

            migrationBuilder.DropTable(
                name: "UtilityPriceHistories");

            migrationBuilder.DropTable(
                name: "UtilityPrices");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Alerts");

            migrationBuilder.DropTable(
                name: "AlertCriteria");

            migrationBuilder.DropTable(
                name: "StateUtilityIndices");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
