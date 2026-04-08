using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WBS.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddSSLCommerzPaymentFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "Donations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "Donations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankTransactionId",
                table: "Donations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardType",
                table: "Donations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "BankTransactionId",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "CardType",
                table: "Donations");
        }
    }
}
