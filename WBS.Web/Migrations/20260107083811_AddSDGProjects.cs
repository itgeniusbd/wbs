using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WBS.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddSDGProjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SDGProjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SDGId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    TitleBn = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionBn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    District = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DistrictBn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Thana = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ThanaBn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Union = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UnionBn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Village = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VillageBn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BeneficiaryCount = table.Column<int>(type: "int", nullable: false),
                    FeaturedImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SDGProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SDGProjects_SDGs_SDGId",
                        column: x => x.SDGId,
                        principalTable: "SDGs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SDGProjectImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SDGProjectId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CaptionBn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SDGProjectImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SDGProjectImages_SDGProjects_SDGProjectId",
                        column: x => x.SDGProjectId,
                        principalTable: "SDGProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SDGProjectImages_SDGProjectId",
                table: "SDGProjectImages",
                column: "SDGProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SDGProjects_SDGId",
                table: "SDGProjects",
                column: "SDGId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SDGProjectImages");

            migrationBuilder.DropTable(
                name: "SDGProjects");
        }
    }
}
