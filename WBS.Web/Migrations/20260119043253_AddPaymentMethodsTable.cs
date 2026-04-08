using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WBS.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentMethodsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameBn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 19, 4, 32, 52, 213, DateTimeKind.Utc).AddTicks(3571));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 19, 4, 32, 52, 213, DateTimeKind.Utc).AddTicks(3574));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 19, 4, 32, 52, 213, DateTimeKind.Utc).AddTicks(3575));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 19, 4, 32, 52, 213, DateTimeKind.Utc).AddTicks(3577));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 19, 4, 32, 52, 213, DateTimeKind.Utc).AddTicks(3578));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 19, 4, 32, 52, 213, DateTimeKind.Utc).AddTicks(3580));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 19, 4, 32, 52, 213, DateTimeKind.Utc).AddTicks(3581));

            migrationBuilder.InsertData(
                table: "PaymentMethods",
                columns: new[] { "Id", "Description", "DisplayOrder", "Icon", "IsActive", "Name", "NameBn" },
                values: new object[,]
                {
                    { 1, "Cash payment", 1, "fas fa-money-bill-wave", true, "Cash", "??? ????" },
                    { 2, "bKash mobile banking", 2, "fab fa-bitcoin", true, "bKash", "?????" },
                    { 3, "Nagad mobile banking", 3, "fas fa-mobile-alt", true, "Nagad", "???" },
                    { 4, "Bank transfer", 4, "fas fa-university", true, "Bank Transfer", "?????? ??????????" },
                    { 5, "Cheque payment", 5, "fas fa-money-check", true, "Cheque", "???" },
                    { 6, "Card payment", 6, "fas fa-credit-card", true, "Credit/Debit Card", "???????/????? ?????" }
                });

            migrationBuilder.UpdateData(
                table: "SmsBalances",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2026, 1, 19, 4, 32, 52, 213, DateTimeKind.Utc).AddTicks(3525));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 19, 3, 48, 59, 99, DateTimeKind.Utc).AddTicks(3207));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 19, 3, 48, 59, 99, DateTimeKind.Utc).AddTicks(3211));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 19, 3, 48, 59, 99, DateTimeKind.Utc).AddTicks(3212));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 19, 3, 48, 59, 99, DateTimeKind.Utc).AddTicks(3213));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 19, 3, 48, 59, 99, DateTimeKind.Utc).AddTicks(3215));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 19, 3, 48, 59, 99, DateTimeKind.Utc).AddTicks(3216));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 19, 3, 48, 59, 99, DateTimeKind.Utc).AddTicks(3217));

            migrationBuilder.UpdateData(
                table: "SmsBalances",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2026, 1, 19, 3, 48, 59, 99, DateTimeKind.Utc).AddTicks(3109));
        }
    }
}
