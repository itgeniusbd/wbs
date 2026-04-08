using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WBS.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNotificationTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotificationTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TemplateType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DonationTypeId = table.Column<int>(type: "int", nullable: true),
                    EmailSubject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SmsContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvailablePlaceholders = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationTemplates_DonationTypes_DonationTypeId",
                        column: x => x.DonationTypeId,
                        principalTable: "DonationTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "NotificationTemplates",
                columns: new[] { "Id", "AvailablePlaceholders", "Category", "CreatedAt", "CreatedBy", "DonationTypeId", "EmailContent", "EmailSubject", "IsActive", "IsDefault", "Name", "SmsContent", "TemplateType", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "{DonorName}, {Amount}, {DonationType}, {TransactionId}", "DonationReceipt", new DateTime(2026, 1, 7, 15, 19, 12, 522, DateTimeKind.Utc).AddTicks(4622), "System", null, null, null, true, true, "Default Donation Receipt SMS", "Dear {DonorName}, Thank you for your generous donation of BDT {Amount} to {DonationType}. Transaction ID: {TransactionId}. May Allah accept your charity. - WBS", "SMS", null },
                    { 2, "{DonorName}, {Amount}, {DonationType}, {TransactionId}, {Date}", "DonationReceipt", new DateTime(2026, 1, 7, 15, 19, 12, 522, DateTimeKind.Utc).AddTicks(4626), "System", null, "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <style>\r\n        body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }\r\n        .container { max-width: 600px; margin: 0 auto; padding: 20px; }\r\n        .header { background: linear-gradient(135deg, #2c5f2d 0%, #1a3a1b 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }\r\n        .logo { max-width: 120px; margin-bottom: 15px; }\r\n        .content { background: #f8f9fa; padding: 30px; }\r\n        .receipt-box { background: white; padding: 20px; border-radius: 8px; margin: 20px 0; border-left: 4px solid #28a745; }\r\n        .amount { font-size: 32px; font-weight: bold; color: #28a745; }\r\n        .footer { text-align: center; padding: 20px; color: #666; font-size: 14px; }\r\n    </style>\r\n</head>\r\n<body>\r\n    <div class='container'>\r\n        <div class='header'>\r\n            <img src='https://yourwebsite.com/images/logo.png' alt='WBS Logo' class='logo' />\r\n            <h1>WBS</h1>\r\n            <p>Thank You for Your Generosity!</p>\r\n        </div>\r\n        \r\n        <div class='content'>\r\n            <h2>Dear {DonorName},</h2>\r\n            <p>Assalamu Alaikum! We are deeply grateful for your generous donation. Your contribution will help us make a positive impact in the community.</p>\r\n            \r\n            <div class='receipt-box'>\r\n                <h3>Donation Receipt</h3>\r\n                <table style='width: 100%;'>\r\n                    <tr><td><strong>Transaction ID:</strong></td><td>{TransactionId}</td></tr>\r\n                    <tr><td><strong>Donation Type:</strong></td><td>{DonationType}</td></tr>\r\n                    <tr><td><strong>Amount:</strong></td><td><span class='amount'>?{Amount}</span></td></tr>\r\n                    <tr><td><strong>Date:</strong></td><td>{Date}</td></tr>\r\n                </table>\r\n            </div>\r\n            \r\n            <p><strong>May Allah accept your charity and bless you abundantly.</strong></p>\r\n            \r\n            <p style='margin-top: 30px;'>If you have any questions, please don't hesitate to contact us.</p>\r\n        </div>\r\n        \r\n        <div class='footer'>\r\n            <p><strong>WBS</strong></p>\r\n            <p>Working for Humanity</p>\r\n            <p>Email: info@wbs.org | Phone: +880 1XXX-XXXXXX</p>\r\n            <p>&copy; 2024 WBS. All rights reserved.</p>\r\n        </div>\r\n    </div>\r\n</body>\r\n</html>", "Thank You for Your Donation - Receipt #{TransactionId}", true, true, "Default Donation Receipt Email", null, "Email", null }
                });

            migrationBuilder.UpdateData(
                table: "SmsBalances",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2026, 1, 7, 15, 19, 12, 522, DateTimeKind.Utc).AddTicks(4586));

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTemplates_DonationTypeId",
                table: "NotificationTemplates",
                column: "DonationTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationTemplates");

            migrationBuilder.UpdateData(
                table: "SmsBalances",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2026, 1, 7, 12, 44, 39, 162, DateTimeKind.Utc).AddTicks(9740));
        }
    }
}
