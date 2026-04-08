using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WBS.Web.Migrations
{
    /// <inheritdoc />
    public partial class MakeTitleOptionalInOtherIncome : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "OtherIncomes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 12, 41, 20, 277, DateTimeKind.Utc).AddTicks(2732));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 12, 41, 20, 277, DateTimeKind.Utc).AddTicks(2735));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 12, 41, 20, 277, DateTimeKind.Utc).AddTicks(2737));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 12, 41, 20, 277, DateTimeKind.Utc).AddTicks(2740));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 12, 41, 20, 277, DateTimeKind.Utc).AddTicks(2741));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 12, 41, 20, 277, DateTimeKind.Utc).AddTicks(2743));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 12, 41, 20, 277, DateTimeKind.Utc).AddTicks(2744));

            migrationBuilder.UpdateData(
                table: "SmsBalances",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2026, 1, 24, 12, 41, 20, 277, DateTimeKind.Utc).AddTicks(2697));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "OtherIncomes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 12, 7, 21, 926, DateTimeKind.Utc).AddTicks(2424));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 12, 7, 21, 926, DateTimeKind.Utc).AddTicks(2428));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 12, 7, 21, 926, DateTimeKind.Utc).AddTicks(2430));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 12, 7, 21, 926, DateTimeKind.Utc).AddTicks(2431));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 12, 7, 21, 926, DateTimeKind.Utc).AddTicks(2433));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 12, 7, 21, 926, DateTimeKind.Utc).AddTicks(2435));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 12, 7, 21, 926, DateTimeKind.Utc).AddTicks(2436));

            migrationBuilder.UpdateData(
                table: "SmsBalances",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2026, 1, 24, 12, 7, 21, 926, DateTimeKind.Utc).AddTicks(2388));
        }
    }
}
