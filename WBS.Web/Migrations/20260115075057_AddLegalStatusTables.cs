using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WBS.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddLegalStatusTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Check if PublishedDate column exists, if not add it
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Publications]') AND name = 'PublishedDate')
                BEGIN
                    ALTER TABLE [Publications] ADD [PublishedDate] datetime2 NULL;
                END
            ");

            migrationBuilder.CreateTable(
                name: "LegalStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CertificateImage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CertificateImageBn = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LegalStatusId = table.Column<int>(type: "int", nullable: false),
                    Authority = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    AuthorityBn = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RegistrationNumberBn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationInfos_LegalStatuses_LegalStatusId",
                        column: x => x.LegalStatusId,
                        principalTable: "LegalStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationInfos_LegalStatusId",
                table: "RegistrationInfos",
                column: "LegalStatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistrationInfos");

            migrationBuilder.DropTable(
                name: "LegalStatuses");

            migrationBuilder.DropColumn(
                name: "PublishedDate",
                table: "Publications");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Publications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PublicationType",
                table: "Publications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "Publications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Publications",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "NotificationTemplates",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 15, 6, 44, 4, 635, DateTimeKind.Utc).AddTicks(5425));

            migrationBuilder.UpdateData(
                table: "NotificationTemplates",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 15, 6, 44, 4, 635, DateTimeKind.Utc).AddTicks(5428));

            migrationBuilder.UpdateData(
                table: "SmsBalances",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2026, 1, 15, 6, 44, 4, 635, DateTimeKind.Utc).AddTicks(5391));
        }
    }
}
