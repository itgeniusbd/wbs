using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WBS.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddSDGAndProgramToDonation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SDGId",
                table: "Donations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProgramId",
                table: "Donations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Donations_SDGId",
                table: "Donations",
                column: "SDGId");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_ProgramId",
                table: "Donations",
                column: "ProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_SDGs_SDGId",
                table: "Donations",
                column: "SDGId",
                principalTable: "SDGs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_SDGPrograms_ProgramId",
                table: "Donations",
                column: "ProgramId",
                principalTable: "SDGPrograms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donations_SDGs_SDGId",
                table: "Donations");

            migrationBuilder.DropForeignKey(
                name: "FK_Donations_SDGPrograms_ProgramId",
                table: "Donations");

            migrationBuilder.DropIndex(
                name: "IX_Donations_SDGId",
                table: "Donations");

            migrationBuilder.DropIndex(
                name: "IX_Donations_ProgramId",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "SDGId",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "ProgramId",
                table: "Donations");
        }
    }
}
