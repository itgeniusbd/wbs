using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WBS.Web.Migrations
{
    /// <inheritdoc />
    public partial class FixDecimalPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 2,
                column: "NameBn",
                value: "?????? ??????");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 2,
                column: "NameBn",
                value: "????????????");
        }
    }
}
