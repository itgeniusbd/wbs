using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WBS.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddUserManagementSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetRoles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AspNetRoles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetRoles",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetRoles",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AspNetRoles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameBn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Module = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    GrantedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 22, 5, 27, 27, 549, DateTimeKind.Utc).AddTicks(2509));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 22, 5, 27, 27, 549, DateTimeKind.Utc).AddTicks(2512));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 22, 5, 27, 27, 549, DateTimeKind.Utc).AddTicks(2514));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 22, 5, 27, 27, 549, DateTimeKind.Utc).AddTicks(2516));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 22, 5, 27, 27, 549, DateTimeKind.Utc).AddTicks(2518));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 22, 5, 27, 27, 549, DateTimeKind.Utc).AddTicks(2520));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 22, 5, 27, 27, 549, DateTimeKind.Utc).AddTicks(2521));

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Action", "Description", "DisplayOrder", "Module", "Name", "NameBn" },
                values: new object[,]
                {
                    { 1, "View", null, 1, "Dashboard", "View Dashboard", "?????????? ?????" },
                    { 2, "View", null, 2, "Users", "View Users", "??????????? ?????" },
                    { 3, "Create", null, 3, "Users", "Create User", "??????????? ???? ????" },
                    { 4, "Edit", null, 4, "Users", "Edit User", "??????????? ???????? ????" },
                    { 5, "Delete", null, 5, "Users", "Delete User", "??????????? ?????" },
                    { 6, "View", null, 6, "Roles", "View Roles", "?????? ?????" },
                    { 7, "Create", null, 7, "Roles", "Create Role", "?????? ???? ????" },
                    { 8, "Edit", null, 8, "Roles", "Edit Role", "?????? ???????? ????" },
                    { 9, "Delete", null, 9, "Roles", "Delete Role", "?????? ?????" },
                    { 10, "View", null, 10, "Donations", "View Donations", "??? ?????" },
                    { 11, "Create", null, 11, "Donations", "Create Donation", "??? ???? ????" },
                    { 12, "Edit", null, 12, "Donations", "Edit Donation", "??? ???????? ????" },
                    { 13, "Delete", null, 13, "Donations", "Delete Donation", "??? ?????" },
                    { 14, "View", null, 14, "Pages", "View Pages", "??? ?????" },
                    { 15, "Create", null, 15, "Pages", "Create Page", "??? ???? ????" },
                    { 16, "Edit", null, 16, "Pages", "Edit Page", "??? ???????? ????" },
                    { 17, "Delete", null, 17, "Pages", "Delete Page", "??? ?????" },
                    { 18, "View", null, 18, "News", "View News", "????? ?????" },
                    { 19, "Create", null, 19, "News", "Create News", "????? ???? ????" },
                    { 20, "Edit", null, 20, "News", "Edit News", "????? ???????? ????" },
                    { 21, "Delete", null, 21, "News", "Delete News", "????? ?????" },
                    { 22, "View", null, 22, "Events", "View Events", "?????? ?????" },
                    { 23, "Create", null, 23, "Events", "Create Event", "?????? ???? ????" },
                    { 24, "Edit", null, 24, "Events", "Edit Event", "?????? ???????? ????" },
                    { 25, "Delete", null, 25, "Events", "Delete Event", "?????? ?????" },
                    { 26, "View", null, 26, "Volunteers", "View Volunteers", "???????????? ?????" },
                    { 27, "Edit", null, 27, "Volunteers", "Edit Volunteer", "???????????? ???????? ????" },
                    { 28, "Delete", null, 28, "Volunteers", "Delete Volunteer", "???????????? ?????" },
                    { 29, "View", null, 29, "Galleries", "View Galleries", "???????? ?????" },
                    { 30, "Create", null, 30, "Galleries", "Create Gallery", "???????? ???? ????" },
                    { 31, "Edit", null, 31, "Galleries", "Edit Gallery", "???????? ???????? ????" },
                    { 32, "Delete", null, 32, "Galleries", "Delete Gallery", "???????? ?????" },
                    { 33, "View", null, 33, "Settings", "View Settings", "?????? ?????" },
                    { 34, "Edit", null, 34, "Settings", "Edit Settings", "?????? ???????? ????" }
                });

            migrationBuilder.UpdateData(
                table: "SmsBalances",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2026, 1, 22, 5, 27, 27, 549, DateTimeKind.Utc).AddTicks(2428));

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AspNetRoles");

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 19, 4, 32, 52, 213, DateTimeKind.Utc).AddTicks(3571));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 19, 4, 32, 52, 213, DateTimeKind.Utc).AddTicks(3574));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 19, 4, 32, 52, 213, DateTimeKind.Utc).AddTicks(3575));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 19, 4, 32, 52, 213, DateTimeKind.Utc).AddTicks(3577));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 19, 4, 32, 52, 213, DateTimeKind.Utc).AddTicks(3578));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 19, 4, 32, 52, 213, DateTimeKind.Utc).AddTicks(3580));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 19, 4, 32, 52, 213, DateTimeKind.Utc).AddTicks(3581));

            migrationBuilder.UpdateData(
                table: "SmsBalances",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2026, 1, 19, 4, 32, 52, 213, DateTimeKind.Utc).AddTicks(3525));
        }
    }
}
