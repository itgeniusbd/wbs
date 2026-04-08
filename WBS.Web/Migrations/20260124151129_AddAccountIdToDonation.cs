using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WBS.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountIdToDonation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Donations",
                type: "int",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Donations_AccountId",
                table: "Donations",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_Accounts_AccountId",
                table: "Donations",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donations_Accounts_AccountId",
                table: "Donations");

            migrationBuilder.DropIndex(
                name: "IX_Donations_AccountId",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Donations");

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 14, 20, 29, 434, DateTimeKind.Utc).AddTicks(1333));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 14, 20, 29, 434, DateTimeKind.Utc).AddTicks(1337));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 14, 20, 29, 434, DateTimeKind.Utc).AddTicks(1339));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 14, 20, 29, 434, DateTimeKind.Utc).AddTicks(1341));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 14, 20, 29, 434, DateTimeKind.Utc).AddTicks(1342));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 14, 20, 29, 434, DateTimeKind.Utc).AddTicks(1344));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 14, 20, 29, 434, DateTimeKind.Utc).AddTicks(1346));

            migrationBuilder.UpdateData(
                table: "SmsBalances",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2026, 1, 24, 14, 20, 29, 434, DateTimeKind.Utc).AddTicks(1294));
        }
    }
}
