﻿// <auto-generated />
using System;
using CompareHare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CompareHare.Api.Migrations
{
    [DbContext(typeof(CompareHareDbContext))]
    partial class CompareHareDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("CompareHare.Domain.Entities.Alert", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AlertCriteriaId");

                    b.Property<string>("AlertOfferHash")
                        .IsRequired()
                        .HasMaxLength(40);

                    b.Property<DateTimeOffset>("CreatedDate");

                    b.Property<DateTimeOffset?>("ModifiedDate");

                    b.Property<int>("StateUtilityIndexId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("AlertCriteriaId");

                    b.HasIndex("StateUtilityIndexId");

                    b.HasIndex("UserId");

                    b.ToTable("Alerts");
                });

            modelBuilder.Entity("CompareHare.Domain.Entities.AlertCriteria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comments")
                        .HasMaxLength(512);

                    b.Property<DateTimeOffset>("CreatedDate");

                    b.Property<bool>("HasBulkDiscounts");

                    b.Property<bool>("HasCancellationFee");

                    b.Property<bool>("HasEnrollmentFee");

                    b.Property<bool>("HasMonthlyFee");

                    b.Property<bool>("HasNetMetering");

                    b.Property<bool>("HasRenewable");

                    b.Property<int>("MaximumMonthLength");

                    b.Property<decimal>("MaximumPrice");

                    b.Property<decimal>("MaximumRenewablePercent");

                    b.Property<int>("MinimumMonthLength");

                    b.Property<decimal>("MinimumPrice");

                    b.Property<decimal>("MinimumRenewablePercent");

                    b.Property<DateTimeOffset?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<bool>("RequiresDeposit");

                    b.Property<string>("StateUtilityIndexHash")
                        .HasMaxLength(40);

                    b.Property<int>("StateUtilityIndexId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("StateUtilityIndexId");

                    b.HasIndex("UserId");

                    b.ToTable("AlertCriteria");
                });

            modelBuilder.Entity("CompareHare.Domain.Entities.PendingAlertNotification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AlertId");

                    b.Property<DateTimeOffset>("CreatedDate");

                    b.Property<DateTimeOffset?>("ModifiedDate");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("AlertId");

                    b.HasIndex("UserId");

                    b.ToTable("PendingAlertNotifications");
                });

            modelBuilder.Entity("CompareHare.Domain.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("CompareHare.Domain.Entities.StateUtilityIndex", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<DateTimeOffset>("CreatedDate");

                    b.Property<string>("LastUpdatedHash")
                        .HasMaxLength(40);

                    b.Property<string>("LoaderDataIdentifier")
                        .HasMaxLength(256);

                    b.Property<DateTimeOffset?>("ModifiedDate");

                    b.Property<int>("UtilityState");

                    b.Property<int>("UtilityType");

                    b.HasKey("Id");

                    b.ToTable("StateUtilityIndices");
                });

            modelBuilder.Entity("CompareHare.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<bool>("Active");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTimeOffset>("CreatedDate");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<DateTime?>("FirstLogin");

                    b.Property<string>("FirstName")
                        .HasMaxLength(255);

                    b.Property<DateTime?>("FullAccessGrantedDate")
                        .HasColumnType("date");

                    b.Property<DateTime?>("LastLogin");

                    b.Property<string>("LastName")
                        .HasMaxLength(255);

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<DateTimeOffset?>("ModifiedDate");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PasswordResetToken")
                        .HasMaxLength(128);

                    b.Property<DateTimeOffset?>("PasswordResetTokenExpirationDate");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("TimeZone")
                        .HasMaxLength(45);

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("CompareHare.Domain.Entities.UtilityPrice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CancellationFee");

                    b.Property<string>("Comments")
                        .HasMaxLength(512);

                    b.Property<DateTimeOffset>("CreatedDate");

                    b.Property<string>("EnrollmentFee");

                    b.Property<string>("FlatRate")
                        .HasMaxLength(120);

                    b.Property<bool>("HasBulkDiscounts");

                    b.Property<bool>("HasCancellationFee");

                    b.Property<bool>("HasEnrollmentFee");

                    b.Property<bool>("HasMonthlyFee");

                    b.Property<bool>("HasNetMetering");

                    b.Property<bool>("HasRenewable");

                    b.Property<bool>("IsIntroductoryPrice");

                    b.Property<DateTimeOffset?>("ModifiedDate");

                    b.Property<string>("MonthlyFee");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<string>("NetMetering");

                    b.Property<string>("OfferUrl")
                        .HasMaxLength(256);

                    b.Property<float>("PricePerUnit");

                    b.Property<string>("PriceStructure")
                        .HasMaxLength(255);

                    b.Property<string>("PriceUnit")
                        .HasMaxLength(50);

                    b.Property<float>("RenewablePercentage");

                    b.Property<bool>("RequiresDeposit");

                    b.Property<int>("StateUtilityIndexId");

                    b.Property<DateTime>("TermEndDate");

                    b.Property<int>("TermMonthLength");

                    b.HasKey("Id");

                    b.HasIndex("StateUtilityIndexId");

                    b.ToTable("UtilityPrices");
                });

            modelBuilder.Entity("CompareHare.Domain.Entities.UtilityPriceHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CancellationFee");

                    b.Property<string>("Comments")
                        .HasMaxLength(512);

                    b.Property<DateTimeOffset>("CreatedDate");

                    b.Property<string>("EnrollmentFee");

                    b.Property<string>("FlatRate")
                        .HasMaxLength(120);

                    b.Property<bool>("HasBulkDiscounts");

                    b.Property<bool>("HasCancellationFee");

                    b.Property<bool>("HasEnrollmentFee");

                    b.Property<bool>("HasMonthlyFee");

                    b.Property<bool>("HasNetMetering");

                    b.Property<bool>("HasRenewable");

                    b.Property<bool>("IsIntroductoryPrice");

                    b.Property<DateTimeOffset?>("ModifiedDate");

                    b.Property<string>("MonthlyFee");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<string>("NetMetering");

                    b.Property<string>("OfferUrl")
                        .HasMaxLength(256);

                    b.Property<float>("PricePerUnit");

                    b.Property<string>("PriceStructure")
                        .HasMaxLength(255);

                    b.Property<string>("PriceUnit")
                        .HasMaxLength(50);

                    b.Property<float>("RenewablePercentage");

                    b.Property<bool>("RequiresDeposit");

                    b.Property<int>("StateUtilityIndexId");

                    b.Property<DateTime>("TermEndDate");

                    b.Property<int>("TermMonthLength");

                    b.HasKey("Id");

                    b.HasIndex("StateUtilityIndexId");

                    b.ToTable("UtilityPriceHistories");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<int>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("CompareHare.Domain.Entities.Alert", b =>
                {
                    b.HasOne("CompareHare.Domain.Entities.AlertCriteria", "AlertCriteria")
                        .WithMany()
                        .HasForeignKey("AlertCriteriaId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CompareHare.Domain.Entities.StateUtilityIndex", "StateUtilityIndex")
                        .WithMany()
                        .HasForeignKey("StateUtilityIndexId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CompareHare.Domain.Entities.User", "User")
                        .WithMany("Alerts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CompareHare.Domain.Entities.AlertCriteria", b =>
                {
                    b.HasOne("CompareHare.Domain.Entities.StateUtilityIndex", "StateUtilityIndex")
                        .WithMany()
                        .HasForeignKey("StateUtilityIndexId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CompareHare.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CompareHare.Domain.Entities.PendingAlertNotification", b =>
                {
                    b.HasOne("CompareHare.Domain.Entities.Alert", "Alert")
                        .WithMany()
                        .HasForeignKey("AlertId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CompareHare.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CompareHare.Domain.Entities.Role", b =>
                {
                    b.HasOne("CompareHare.Domain.Entities.User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("CompareHare.Domain.Entities.UtilityPrice", b =>
                {
                    b.HasOne("CompareHare.Domain.Entities.StateUtilityIndex", "StateUtilityIndex")
                        .WithMany()
                        .HasForeignKey("StateUtilityIndexId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CompareHare.Domain.Entities.UtilityPriceHistory", b =>
                {
                    b.HasOne("CompareHare.Domain.Entities.StateUtilityIndex", "StateUtilityIndex")
                        .WithMany()
                        .HasForeignKey("StateUtilityIndexId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("CompareHare.Domain.Entities.Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("CompareHare.Domain.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("CompareHare.Domain.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("CompareHare.Domain.Entities.Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CompareHare.Domain.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("CompareHare.Domain.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
