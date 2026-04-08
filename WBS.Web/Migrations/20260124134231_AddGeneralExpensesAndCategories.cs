using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WBS.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddGeneralExpensesAndCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Safely rename column if it exists
            migrationBuilder.Sql(@"
                IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
                          WHERE TABLE_NAME = 'OtherIncomes' AND COLUMN_NAME = 'Source')
                BEGIN
                    EXEC sp_rename 'OtherIncomes.Source', 'Account', 'COLUMN';
                END
            ");

            migrationBuilder.CreateTable(
                name: "ExpenseCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NameBn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeneralExpenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpenseCategoryId = table.Column<int>(type: "int", nullable: false),
                    Account = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ExpenseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralExpenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralExpenses_ExpenseCategories_ExpenseCategoryId",
                        column: x => x.ExpenseCategoryId,
                        principalTable: "ExpenseCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 13, 42, 30, 374, DateTimeKind.Utc).AddTicks(2032));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 13, 42, 30, 374, DateTimeKind.Utc).AddTicks(2035));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 13, 42, 30, 374, DateTimeKind.Utc).AddTicks(2037));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 13, 42, 30, 374, DateTimeKind.Utc).AddTicks(2038));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 13, 42, 30, 374, DateTimeKind.Utc).AddTicks(2117));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 13, 42, 30, 374, DateTimeKind.Utc).AddTicks(2119));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 13, 42, 30, 374, DateTimeKind.Utc).AddTicks(2120));

            migrationBuilder.UpdateData(
                table: "SmsBalances",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2026, 1, 24, 13, 42, 30, 374, DateTimeKind.Utc).AddTicks(1997));

            migrationBuilder.CreateIndex(
                name: "IX_GeneralExpenses_ExpenseCategoryId",
                table: "GeneralExpenses",
                column: "ExpenseCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneralExpenses");

            migrationBuilder.DropTable(
                name: "ExpenseCategories");

            migrationBuilder.RenameColumn(
                name: "Account",
                table: "OtherIncomes",
                newName: "Source");

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 12, 52, 55, 551, DateTimeKind.Utc).AddTicks(9327));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 12, 52, 55, 551, DateTimeKind.Utc).AddTicks(9329));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 12, 52, 55, 551, DateTimeKind.Utc).AddTicks(9331));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 12, 52, 55, 551, DateTimeKind.Utc).AddTicks(9333));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 12, 52, 55, 551, DateTimeKind.Utc).AddTicks(9334));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 12, 52, 55, 551, DateTimeKind.Utc).AddTicks(9336));

            migrationBuilder.UpdateData(
                table: "DonorTypeCategories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 24, 12, 52, 55, 551, DateTimeKind.Utc).AddTicks(9338));

            migrationBuilder.UpdateData(
                table: "SmsBalances",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2026, 1, 24, 12, 52, 55, 551, DateTimeKind.Utc).AddTicks(9283));
        }
    }
}
