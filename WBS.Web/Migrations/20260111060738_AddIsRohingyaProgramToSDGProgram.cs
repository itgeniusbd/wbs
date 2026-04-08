using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WBS.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddIsRohingyaProgramToSDGProgram : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRohingyaProgram",
                table: "SDGPrograms",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRohingyaProgram",
                table: "SDGPrograms");
        }
    }
}
