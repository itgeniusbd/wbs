using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WBS.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddAllSDGs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 1,
                column: "NameBn",
                value: "????????? ?????");

            migrationBuilder.UpdateData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 6,
                column: "NameBn",
                value: "????????? ???? ? ??????????");

            migrationBuilder.InsertData(
                table: "SDGs",
                columns: new[] { "Id", "Color", "Description", "DescriptionBn", "Icon", "IsActive", "Name", "NameBn", "Number" },
                values: new object[,]
                {
                    { 7, "#FCC30B", null, null, null, true, "Affordable and Clean Energy", "???????? ? ????????? ?????", 7 },
                    { 8, "#A21942", null, null, null, true, "Decent Work and Economic Growth", "??????? ??? ? ????????? ?????????", 8 },
                    { 9, "#FD6925", null, null, null, true, "Industry, Innovation and Infrastructure", "?????, ??????? ? ????????", 9 },
                    { 10, "#DD1367", null, null, null, true, "Reduced Inequalities", "????? ?????", 10 },
                    { 11, "#FD9D24", null, null, null, true, "Sustainable Cities and Communities", "????? ??? ? ????????", 11 },
                    { 12, "#BF8B2E", null, null, null, true, "Responsible Consumption and Production", "??????????? ??? ? ??????", 12 },
                    { 13, "#3F7E44", null, null, null, true, "Climate Action", "??????? ???????", 13 },
                    { 14, "#0A97D9", null, null, null, true, "Life Below Water", "????? ???? ????", 14 },
                    { 15, "#56C02B", null, null, null, true, "Life on Land", "?????? ????", 15 },
                    { 16, "#00689D", null, null, null, true, "Peace, Justice and Strong Institutions", "??????, ??????????? ? ????????? ??????????", 16 },
                    { 17, "#19486A", null, null, null, true, "Partnerships for the Goals", "??????? ???????????", 17 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.UpdateData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 1,
                column: "NameBn",
                value: "????????? ??????");

            migrationBuilder.UpdateData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 6,
                column: "NameBn",
                value: "??????? ???? ? ??????????");
        }
    }
}
