using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WBS.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddDistrictAndUpazilaToSDGProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DistrictId",
                table: "SDGProjects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpazilaId",
                table: "SDGProjects",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "SmsBalances",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2026, 1, 18, 9, 48, 14, 519, DateTimeKind.Utc).AddTicks(125));

            migrationBuilder.CreateIndex(
                name: "IX_Upazilas_Name_DistrictId",
                table: "Upazilas",
                columns: new[] { "Name", "DistrictId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SDGProjects_DistrictId",
                table: "SDGProjects",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_SDGProjects_UpazilaId",
                table: "SDGProjects",
                column: "UpazilaId");

            migrationBuilder.AddForeignKey(
                name: "FK_SDGProjects_Districts_DistrictId",
                table: "SDGProjects",
                column: "DistrictId",
                principalTable: "Districts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SDGProjects_Upazilas_UpazilaId",
                table: "SDGProjects",
                column: "UpazilaId",
                principalTable: "Upazilas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SDGProjects_Districts_DistrictId",
                table: "SDGProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_SDGProjects_Upazilas_UpazilaId",
                table: "SDGProjects");

            migrationBuilder.DropIndex(
                name: "IX_Upazilas_Name_DistrictId",
                table: "Upazilas");

            migrationBuilder.DropIndex(
                name: "IX_SDGProjects_DistrictId",
                table: "SDGProjects");

            migrationBuilder.DropIndex(
                name: "IX_SDGProjects_UpazilaId",
                table: "SDGProjects");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "SDGProjects");

            migrationBuilder.DropColumn(
                name: "UpazilaId",
                table: "SDGProjects");

            migrationBuilder.UpdateData(
                table: "SmsBalances",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2026, 1, 17, 8, 24, 54, 422, DateTimeKind.Utc).AddTicks(9956));
        }
    }
}
