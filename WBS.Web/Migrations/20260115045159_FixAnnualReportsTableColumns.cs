using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WBS.Web.Migrations
{
    /// <inheritdoc />
    public partial class FixAnnualReportsTableColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Check and add CreatedAt column if it doesn't exist
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AnnualReports') AND name = 'CreatedAt')
                BEGIN
                    ALTER TABLE AnnualReports ADD CreatedAt datetime2 NOT NULL DEFAULT GETUTCDATE()
                END
            ");
            
            // Check and add Description column if it doesn't exist
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AnnualReports') AND name = 'Description')
                BEGIN
                    ALTER TABLE AnnualReports ADD Description nvarchar(max) NULL
                END
            ");
            
            // Check and add DescriptionBn column if it doesn't exist
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AnnualReports') AND name = 'DescriptionBn')
                BEGIN
                    ALTER TABLE AnnualReports ADD DescriptionBn nvarchar(max) NULL
                END
            ");
            
            // Check and add CoverImage column if it doesn't exist
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AnnualReports') AND name = 'CoverImage')
                BEGIN
                    ALTER TABLE AnnualReports ADD CoverImage nvarchar(max) NULL
                END
            ");
            
            // Check and add IsActive column if it doesn't exist
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AnnualReports') AND name = 'IsActive')
                BEGIN
                    ALTER TABLE AnnualReports ADD IsActive bit NOT NULL DEFAULT 1
                END
            ");
            
            migrationBuilder.UpdateData(
                table: "NotificationTemplates",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 15, 4, 51, 57, 990, DateTimeKind.Utc).AddTicks(604));

            migrationBuilder.UpdateData(
                table: "NotificationTemplates",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 15, 4, 51, 57, 990, DateTimeKind.Utc).AddTicks(607));

            migrationBuilder.UpdateData(
                table: "SmsBalances",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2026, 1, 15, 4, 51, 57, 990, DateTimeKind.Utc).AddTicks(570));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove columns if they exist
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AnnualReports') AND name = 'CreatedAt')
                BEGIN
                    ALTER TABLE AnnualReports DROP COLUMN CreatedAt
                END
            ");
            
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AnnualReports') AND name = 'Description')
                BEGIN
                    ALTER TABLE AnnualReports DROP COLUMN Description
                END
            ");
            
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AnnualReports') AND name = 'DescriptionBn')
                BEGIN
                    ALTER TABLE AnnualReports DROP COLUMN DescriptionBn
                END
            ");
            
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AnnualReports') AND name = 'CoverImage')
                BEGIN
                    ALTER TABLE AnnualReports DROP COLUMN CoverImage
                END
            ");
            
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AnnualReports') AND name = 'IsActive')
                BEGIN
                    ALTER TABLE AnnualReports DROP COLUMN IsActive
                END
            ");
            
            migrationBuilder.UpdateData(
                table: "NotificationTemplates",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 14, 12, 23, 2, 687, DateTimeKind.Utc).AddTicks(5474));

            migrationBuilder.UpdateData(
                table: "NotificationTemplates",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 14, 12, 23, 2, 687, DateTimeKind.Utc).AddTicks(5477));

            migrationBuilder.UpdateData(
                table: "SmsBalances",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2026, 1, 14, 12, 23, 2, 687, DateTimeKind.Utc).AddTicks(5445));
        }
    }
}
