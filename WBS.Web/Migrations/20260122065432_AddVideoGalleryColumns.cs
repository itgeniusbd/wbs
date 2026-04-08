using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WBS.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddVideoGalleryColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Check if table exists, if not create it
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'VideoGalleries')
                BEGIN
                    CREATE TABLE [VideoGalleries] (
                        [Id] int NOT NULL IDENTITY,
                        [Title] nvarchar(200) NOT NULL,
                        [TitleBn] nvarchar(200) NULL,
                        [Description] nvarchar(max) NULL,
                        [DescriptionBn] nvarchar(max) NULL,
                        [YouTubeUrl] nvarchar(500) NOT NULL,
                        [YouTubeVideoId] nvarchar(100) NULL,
                        [ThumbnailUrl] nvarchar(500) NULL,
                        [DisplayOrder] int NOT NULL,
                        [IsActive] bit NOT NULL,
                        [IsFeatured] bit NOT NULL,
                        [CreatedAt] datetime2 NOT NULL,
                        [UpdatedAt] datetime2 NULL,
                        [CreatedBy] nvarchar(100) NULL,
                        CONSTRAINT [PK_VideoGalleries] PRIMARY KEY ([Id])
                    );
                END
                ELSE
                BEGIN
                    -- Add missing columns if they don't exist
                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[VideoGalleries]') AND name = 'CreatedBy')
                        ALTER TABLE [VideoGalleries] ADD [CreatedBy] nvarchar(100) NULL;
                    
                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[VideoGalleries]') AND name = 'DisplayOrder')
                        ALTER TABLE [VideoGalleries] ADD [DisplayOrder] int NOT NULL DEFAULT 0;
                    
                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[VideoGalleries]') AND name = 'IsFeatured')
                        ALTER TABLE [VideoGalleries] ADD [IsFeatured] bit NOT NULL DEFAULT 0;
                    
                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[VideoGalleries]') AND name = 'ThumbnailUrl')
                        ALTER TABLE [VideoGalleries] ADD [ThumbnailUrl] nvarchar(500) NULL;
                    
                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[VideoGalleries]') AND name = 'UpdatedAt')
                        ALTER TABLE [VideoGalleries] ADD [UpdatedAt] datetime2 NULL;
                    
                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[VideoGalleries]') AND name = 'YouTubeUrl')
                        ALTER TABLE [VideoGalleries] ADD [YouTubeUrl] nvarchar(500) NOT NULL DEFAULT '';
                    
                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[VideoGalleries]') AND name = 'YouTubeVideoId')
                        ALTER TABLE [VideoGalleries] ADD [YouTubeVideoId] nvarchar(100) NULL;
                END
            ");

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 22, 6, 54, 31, 255, DateTimeKind.Utc).AddTicks(8152));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 22, 6, 54, 31, 255, DateTimeKind.Utc).AddTicks(8155));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 22, 6, 54, 31, 255, DateTimeKind.Utc).AddTicks(8157));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 22, 6, 54, 31, 255, DateTimeKind.Utc).AddTicks(8159));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 22, 6, 54, 31, 255, DateTimeKind.Utc).AddTicks(8160));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 22, 6, 54, 31, 255, DateTimeKind.Utc).AddTicks(8162));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 22, 6, 54, 31, 255, DateTimeKind.Utc).AddTicks(8164));

            migrationBuilder.UpdateData(
                table: "SmsBalances",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2026, 1, 22, 6, 54, 31, 255, DateTimeKind.Utc).AddTicks(8114));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Only drop columns if they were added by this migration
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[VideoGalleries]') AND name = 'CreatedBy')
                    ALTER TABLE [VideoGalleries] DROP COLUMN [CreatedBy];
                
                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[VideoGalleries]') AND name = 'DisplayOrder')
                    ALTER TABLE [VideoGalleries] DROP COLUMN [DisplayOrder];
                
                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[VideoGalleries]') AND name = 'IsFeatured')
                    ALTER TABLE [VideoGalleries] DROP COLUMN [IsFeatured];
                
                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[VideoGalleries]') AND name = 'ThumbnailUrl')
                    ALTER TABLE [VideoGalleries] DROP COLUMN [ThumbnailUrl];
                
                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[VideoGalleries]') AND name = 'UpdatedAt')
                    ALTER TABLE [VideoGalleries] DROP COLUMN [UpdatedAt];
                
                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[VideoGalleries]') AND name = 'YouTubeUrl')
                    ALTER TABLE [VideoGalleries] DROP COLUMN [YouTubeUrl];
                
                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[VideoGalleries]') AND name = 'YouTubeVideoId')
                    ALTER TABLE [VideoGalleries] DROP COLUMN [YouTubeVideoId];
            ");

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 22, 6, 4, 12, 302, DateTimeKind.Utc).AddTicks(8548));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 22, 6, 4, 12, 302, DateTimeKind.Utc).AddTicks(8551));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 22, 6, 4, 12, 302, DateTimeKind.Utc).AddTicks(8553));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 22, 6, 4, 12, 302, DateTimeKind.Utc).AddTicks(8555));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 22, 6, 4, 12, 302, DateTimeKind.Utc).AddTicks(8556));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 22, 6, 4, 12, 302, DateTimeKind.Utc).AddTicks(8558));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 22, 6, 4, 12, 302, DateTimeKind.Utc).AddTicks(8559));

            migrationBuilder.UpdateData(
                table: "SmsBalances",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2026, 1, 22, 6, 4, 12, 302, DateTimeKind.Utc).AddTicks(8512));
        }
    }
}
