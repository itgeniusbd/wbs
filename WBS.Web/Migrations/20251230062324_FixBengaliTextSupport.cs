using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WBS.Web.Migrations
{
    /// <inheritdoc />
    public partial class FixBengaliTextSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ButtonTextBn",
                table: "Sliders",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "DonationTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "NameBn",
                value: "?????? ????????");

            migrationBuilder.UpdateData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 1,
                column: "NameBn",
                value: "????????? ?????");

            migrationBuilder.UpdateData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 4,
                column: "NameBn",
                value: "????? ??????");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ButtonTextBn",
                table: "Sliders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "DonationTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "NameBn",
                value: "???? ????????");

            migrationBuilder.UpdateData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 1,
                column: "NameBn",
                value: "????????? ???????");

            migrationBuilder.UpdateData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 4,
                column: "NameBn",
                value: "???????? ??????");
        }
    }
}
