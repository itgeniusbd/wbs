using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WBS.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddDonorTypeCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DonorTypeCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameBn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DescriptionBn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonorTypeCategories", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "DonorTypeCategories",
                columns: new[] { "Id", "CreatedAt", "Description", "DescriptionBn", "DisplayOrder", "IsActive", "IsVisible", "Name", "NameBn", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 19, 3, 48, 59, 99, DateTimeKind.Utc).AddTicks(3207), "Regular Donor", null, 1, true, true, "Regular", "?????? ????", null },
                    { 2, new DateTime(2026, 1, 19, 3, 48, 59, 99, DateTimeKind.Utc).AddTicks(3211), "Monthly recurring donor", null, 2, true, true, "Monthly", "????? ????", null },
                    { 3, new DateTime(2026, 1, 19, 3, 48, 59, 99, DateTimeKind.Utc).AddTicks(3212), "Daily recurring donor", null, 3, true, true, "Daily", "????? ????", null },
                    { 4, new DateTime(2026, 1, 19, 3, 48, 59, 99, DateTimeKind.Utc).AddTicks(3213), "Yearly recurring donor", null, 4, true, true, "Yearly", "??????? ????", null },
                    { 5, new DateTime(2026, 1, 19, 3, 48, 59, 99, DateTimeKind.Utc).AddTicks(3215), "Lifetime donor", null, 5, true, true, "Lifetime", "???????? ????", null },
                    { 6, new DateTime(2026, 1, 19, 3, 48, 59, 99, DateTimeKind.Utc).AddTicks(3216), "Corporate or institutional donor", null, 6, true, true, "Corporate", "????????????? ????", null },
                    { 7, new DateTime(2026, 1, 19, 3, 48, 59, 99, DateTimeKind.Utc).AddTicks(3217), "One-time donor", null, 7, true, true, "One Time", "??????? ????", null }
                });

            migrationBuilder.UpdateData(
                table: "SmsBalances",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2026, 1, 19, 3, 48, 59, 99, DateTimeKind.Utc).AddTicks(3109));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DonorTypeCategories");

            migrationBuilder.UpdateData(
                table: "SmsBalances",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2026, 1, 18, 10, 18, 49, 349, DateTimeKind.Utc).AddTicks(8798));
        }
    }
}
