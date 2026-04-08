using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WBS.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentFieldsToDonation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BankTransactionId",
                table: "Donations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardType",
                table: "Donations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "Donations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "Donations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                column: "AccountCreateDate",
                value: new DateTime(2026, 1, 25, 7, 54, 15, 948, DateTimeKind.Utc).AddTicks(7771));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 25, 7, 54, 15, 948, DateTimeKind.Utc).AddTicks(7628));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 25, 7, 54, 15, 948, DateTimeKind.Utc).AddTicks(7631));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 25, 7, 54, 15, 948, DateTimeKind.Utc).AddTicks(7633));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 25, 7, 54, 15, 948, DateTimeKind.Utc).AddTicks(7634));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 25, 7, 54, 15, 948, DateTimeKind.Utc).AddTicks(7636));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 25, 7, 54, 15, 948, DateTimeKind.Utc).AddTicks(7637));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 25, 7, 54, 15, 948, DateTimeKind.Utc).AddTicks(7639));

            migrationBuilder.UpdateData(
                table: "SmsBalances",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2026, 1, 25, 7, 54, 15, 948, DateTimeKind.Utc).AddTicks(7583));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankTransactionId",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "CardType",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "Donations");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                column: "AccountCreateDate",
                value: new DateTime(2026, 1, 25, 5, 9, 0, 510, DateTimeKind.Utc).AddTicks(9941));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 25, 5, 9, 0, 510, DateTimeKind.Utc).AddTicks(9838));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 25, 5, 9, 0, 510, DateTimeKind.Utc).AddTicks(9841));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 25, 5, 9, 0, 510, DateTimeKind.Utc).AddTicks(9842));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 25, 5, 9, 0, 510, DateTimeKind.Utc).AddTicks(9843));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 25, 5, 9, 0, 510, DateTimeKind.Utc).AddTicks(9845));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 25, 5, 9, 0, 510, DateTimeKind.Utc).AddTicks(9846));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 25, 5, 9, 0, 510, DateTimeKind.Utc).AddTicks(9847));

            migrationBuilder.UpdateData(
                table: "SmsBalances",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2026, 1, 25, 5, 9, 0, 510, DateTimeKind.Utc).AddTicks(9807));
        }
    }
}
