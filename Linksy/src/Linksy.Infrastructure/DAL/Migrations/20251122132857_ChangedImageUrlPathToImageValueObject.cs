using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Linksy.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangedImageUrlPathToImageValueObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrlPath",
                table: "LandingPageItems",
                newName: "Image_UrlPath");

            migrationBuilder.AddColumn<string>(
                name: "Image_FileName",
                table: "LandingPageItems",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image_FileName",
                table: "LandingPageItems");

            migrationBuilder.RenameColumn(
                name: "Image_UrlPath",
                table: "LandingPageItems",
                newName: "ImageUrlPath");
        }
    }
}
