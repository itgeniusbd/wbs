using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WBS.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddOrganizationAndPolicyFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrganizationFullName",
                table: "SiteSettings",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationFullNameBn",
                table: "SiteSettings",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegistrationNumber",
                table: "SiteSettings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegistrationType",
                table: "SiteSettings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EstablishedYear",
                table: "SiteSettings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationType",
                table: "SiteSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationTypeBn",
                table: "SiteSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManagementInfo",
                table: "SiteSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManagementInfoBn",
                table: "SiteSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefundPolicyTimeframe",
                table: "SiteSettings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefundPolicyTimeframeBn",
                table: "SiteSettings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentGatewayBanner",
                table: "SiteSettings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrganizationFullName",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "OrganizationFullNameBn",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "RegistrationNumber",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "RegistrationType",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "EstablishedYear",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "OrganizationType",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "OrganizationTypeBn",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "ManagementInfo",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "ManagementInfoBn",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "RefundPolicyTimeframe",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "RefundPolicyTimeframeBn",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "PaymentGatewayBanner",
                table: "SiteSettings");
        }
    }
}
