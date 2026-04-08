using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WBS.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddSocialMediaToPartners : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add new social media columns
            migrationBuilder.AddColumn<string>(
                name: "FacebookUrl",
                table: "Partners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwitterUrl",
                table: "Partners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedInUrl",
                table: "Partners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstagramUrl",
                table: "Partners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YouTubeUrl",
                table: "Partners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "NotificationTemplates",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 15, 9, 34, 49, 548, DateTimeKind.Utc).AddTicks(1421));

            migrationBuilder.UpdateData(
                table: "NotificationTemplates",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 15, 9, 34, 49, 548, DateTimeKind.Utc).AddTicks(1425));

            migrationBuilder.UpdateData(
                table: "SmsBalances",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2026, 1, 15, 9, 34, 49, 548, DateTimeKind.Utc).AddTicks(1383));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FacebookUrl",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "TwitterUrl",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "LinkedInUrl",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "InstagramUrl",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "YouTubeUrl",
                table: "Partners");

            migrationBuilder.UpdateData(
                table: "NotificationTemplates",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 15, 7, 50, 56, 262, DateTimeKind.Utc).AddTicks(9960));

            migrationBuilder.UpdateData(
                table: "NotificationTemplates",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 15, 7, 50, 56, 262, DateTimeKind.Utc).AddTicks(9963));

            migrationBuilder.UpdateData(
                table: "SmsBalances",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2026, 1, 15, 7, 50, 56, 262, DateTimeKind.Utc).AddTicks(9907));
        }
    }
}
