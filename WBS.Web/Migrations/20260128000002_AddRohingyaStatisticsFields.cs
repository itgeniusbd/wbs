using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WBS.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddRohingyaStatisticsFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RohingyaCampsReached",
                table: "SiteSettings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RohingyaTotalBeneficiaries",
                table: "SiteSettings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RohingyaActivePrograms",
                table: "SiteSettings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RohingyaEventsConducted",
                table: "SiteSettings",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RohingyaCampsReached",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "RohingyaTotalBeneficiaries",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "RohingyaActivePrograms",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "RohingyaEventsConducted",
                table: "SiteSettings");
        }
    }
}
