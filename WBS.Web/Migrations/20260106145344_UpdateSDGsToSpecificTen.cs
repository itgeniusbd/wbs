using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WBS.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSDGsToSpecificTen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 5);

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
                keyValue: 12);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 1,
                column: "NameBn",
                value: "????????? ?????");

            migrationBuilder.InsertData(
                table: "SDGs",
                columns: new[] { "Id", "Color", "Description", "DescriptionBn", "Icon", "IsActive", "Name", "NameBn", "Number" },
                values: new object[,]
                {
                    { 5, "#FF3A21", null, null, null, true, "Gender Equality", "????? ????", 5 },
                    { 8, "#A21942", null, null, null, true, "Decent Work and Economic Growth", "??????? ??? ? ????????? ?????????", 8 },
                    { 9, "#FD6925", null, null, null, true, "Industry, Innovation and Infrastructure", "?????, ??????? ? ????????", 9 },
                    { 10, "#DD1367", null, null, null, true, "Reduced Inequalities", "????? ?????", 10 },
                    { 12, "#BF8B2E", null, null, null, true, "Responsible Consumption and Production", "??????????? ??? ? ??????", 12 },
                    { 16, "#00689D", null, null, null, true, "Peace, Justice and Strong Institutions", "??????, ??????????? ? ????????? ??????????", 16 },
                    { 17, "#19486A", null, null, null, true, "Partnerships for the Goals", "??????? ???????????", 17 }
                });
        }
    }
}
