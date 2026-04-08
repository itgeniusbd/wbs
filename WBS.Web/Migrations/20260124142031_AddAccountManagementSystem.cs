using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WBS.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountManagementSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Step 1: Create Accounts table first
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AccountNameBn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AccountBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total_IN = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total_OUT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total_Income = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total_Expense = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Deleted_Income = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Deleted_Expense = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Default_Status = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DescriptionBn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AccountType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BranchName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AccountCreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            // Step 2: Insert default accounts
            migrationBuilder.Sql(@"
                INSERT INTO Accounts (AccountName, AccountNameBn, AccountBalance, Total_IN, Total_OUT, Total_Income, Total_Expense, 
                                      Deleted_Income, Deleted_Expense, Default_Status, IsActive, AccountType, DisplayOrder, AccountCreateDate)
                VALUES 
                ('Main Account', 'প্রধান একাউন্ট', 0, 0, 0, 0, 0, 0, 0, 1, 1, 'Cash', 1, GETUTCDATE()),
                ('Cash', 'নগদ', 0, 0, 0, 0, 0, 0, 0, 0, 1, 'Cash', 2, GETUTCDATE()),
                ('Bank Account', 'ব্যাংক একাউন্ট', 0, 0, 0, 0, 0, 0, 0, 0, 1, 'Bank', 3, GETUTCDATE()),
                ('bKash', 'বিকাশ', 0, 0, 0, 0, 0, 0, 0, 0, 1, 'Mobile Banking', 4, GETUTCDATE()),
                ('Nagad', 'নগদ', 0, 0, 0, 0, 0, 0, 0, 0, 1, 'Mobile Banking', 5, GETUTCDATE())
            ");

            // Step 3: Rename Account columns to Account_Old temporarily
            migrationBuilder.RenameColumn(
                name: "Account",
                table: "OtherIncomes",
                newName: "Account_Old");

            migrationBuilder.RenameColumn(
                name: "Account",
                table: "GeneralExpenses",
                newName: "Account_Old");

            // Step 4: Add new AccountId columns
            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "OtherIncomes",
                type: "int",
                nullable: false,
                defaultValue: 1); // Default to Main Account

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "GeneralExpenses",
                type: "int",
                nullable: false,
                defaultValue: 1); // Default to Main Account

            // Step 5: Drop old Account_Old columns
            migrationBuilder.DropColumn(
                name: "Account_Old",
                table: "OtherIncomes");

            migrationBuilder.DropColumn(
                name: "Account_Old",
                table: "GeneralExpenses");

            // Step 6: Create AccountTransactions table
            migrationBuilder.CreateTable(
                name: "AccountTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BalanceBefore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BalanceAfter = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReferenceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountTransactions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_OtherIncomes_AccountId",
                table: "OtherIncomes",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralExpenses_AccountId",
                table: "GeneralExpenses",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountTransactions_AccountId",
                table: "AccountTransactions",
                column: "AccountId");

            // Step 7: Add foreign keys
            migrationBuilder.AddForeignKey(
                name: "FK_GeneralExpenses_Accounts_AccountId",
                table: "GeneralExpenses",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OtherIncomes_Accounts_AccountId",
                table: "OtherIncomes",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralExpenses_Accounts_AccountId",
                table: "GeneralExpenses");

            migrationBuilder.DropForeignKey(
                name: "FK_OtherIncomes_Accounts_AccountId",
                table: "OtherIncomes");

            migrationBuilder.DropTable(
                name: "AccountTransactions");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_OtherIncomes_AccountId",
                table: "OtherIncomes");

            migrationBuilder.DropIndex(
                name: "IX_GeneralExpenses_AccountId",
                table: "GeneralExpenses");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "OtherIncomes");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "GeneralExpenses");

            migrationBuilder.AddColumn<string>(
                name: "Account",
                table: "OtherIncomes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Account",
                table: "GeneralExpenses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

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
        }
    }
}
