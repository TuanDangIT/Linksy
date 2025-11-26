using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Linksy.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangedNameFromIsActiveToIsPublishedForLandingPageEntityAndAlsoChangedIsActiveImplementationForOtherEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "QrCodes");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Barcodes");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "LaningPages",
                newName: "IsPublished");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "UmtParameters",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "UmtParameters");

            migrationBuilder.RenameColumn(
                name: "IsPublished",
                table: "LaningPages",
                newName: "IsActive");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "QrCodes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Barcodes",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
