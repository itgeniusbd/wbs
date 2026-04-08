using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WBS.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddImageAndDisplayOrderToSDG : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "SDGs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "SDGs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DisplayOrder", "Image" },
                values: new object[] { 0, null });

            migrationBuilder.UpdateData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DisplayOrder", "Image" },
                values: new object[] { 0, null });

            migrationBuilder.UpdateData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DisplayOrder", "Image" },
                values: new object[] { 0, null });

            migrationBuilder.UpdateData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DisplayOrder", "Image" },
                values: new object[] { 0, null });

            migrationBuilder.UpdateData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DisplayOrder", "Image" },
                values: new object[] { 0, null });

            migrationBuilder.UpdateData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "DisplayOrder", "Image" },
                values: new object[] { 0, null });

            migrationBuilder.UpdateData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "DisplayOrder", "Image" },
                values: new object[] { 0, null });

            migrationBuilder.UpdateData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "DisplayOrder", "Image" },
                values: new object[] { 0, null });

            migrationBuilder.UpdateData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "DisplayOrder", "Image" },
                values: new object[] { 0, null });

            migrationBuilder.UpdateData(
                table: "SDGs",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "DisplayOrder", "Image" },
                values: new object[] { 0, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "SDGs");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "SDGs");
        }
    }
}
