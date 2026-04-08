using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WBS.Web.Migrations
{
    /// <inheritdoc />
    public partial class SeedDefaultAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccountBalance", "AccountCreateDate", "AccountName", "AccountNameBn", "AccountNumber", "AccountType", "BankName", "BranchName", "CreatedBy", "Default_Status", "Deleted_Expense", "Deleted_Income", "Description", "DescriptionBn", "DisplayOrder", "IsActive", "Total_Expense", "Total_IN", "Total_Income", "Total_OUT", "UpdatedAt" },
                values: new object[] { 1, 0m, new DateTime(2026, 1, 25, 5, 9, 0, 510, DateTimeKind.Utc).AddTicks(9941), "Main Account", "???? ??????", null, "Cash", null, null, null, true, 0m, 0m, "Default main account for donations", "?????? ????? ???? ???? ??????", 1, true, 0m, 0m, 0m, 0m, null });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 15, 11, 27, 583, DateTimeKind.Utc).AddTicks(1420));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 15, 11, 27, 583, DateTimeKind.Utc).AddTicks(1423));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 15, 11, 27, 583, DateTimeKind.Utc).AddTicks(1425));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 15, 11, 27, 583, DateTimeKind.Utc).AddTicks(1426));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 15, 11, 27, 583, DateTimeKind.Utc).AddTicks(1428));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 15, 11, 27, 583, DateTimeKind.Utc).AddTicks(1429));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 15, 11, 27, 583, DateTimeKind.Utc).AddTicks(1431));

            migrationBuilder.UpdateData(
                table: "SmsBalances",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2026, 1, 24, 15, 11, 27, 583, DateTimeKind.Utc).AddTicks(1388));
        }
    }
}
