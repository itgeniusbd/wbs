using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WBS.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBengaliSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DonationTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "NameBn",
                value: "??????? ????????");

            migrationBuilder.UpdateData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 1,
                column: "NameBn",
                value: "????????? ??????");

            migrationBuilder.UpdateData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 4,
                column: "NameBn",
                value: "???????? ??????");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
