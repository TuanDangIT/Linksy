using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Linksy.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomColumnNamesForChildLandingPageItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VideoUrl",
                table: "LandingPageItems",
                newName: "YoutubeLandingPageItem_VideoUrl");

            migrationBuilder.RenameColumn(
                name: "FontColor",
                table: "LandingPageItems",
                newName: "TextLandingPageItem_FontColor");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "LandingPageItems",
                newName: "TextLandingPageItem_Content");

            migrationBuilder.RenameColumn(
                name: "BackgroundColor",
                table: "LandingPageItems",
                newName: "TextLandingPageItem_BackgroundColor");

            migrationBuilder.RenameColumn(
                name: "AltText",
                table: "LandingPageItems",
                newName: "ImageLandingPageItem_AltText");

            migrationBuilder.RenameColumn(
                name: "Image_UrlPath",
                table: "LandingPageItems",
                newName: "ImageLandingPageItem_Image_UrlPath");

            migrationBuilder.RenameColumn(
                name: "Image_FileName",
                table: "LandingPageItems",
                newName: "ImageLandingPageItem_Image_FileName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "YoutubeLandingPageItem_VideoUrl",
                table: "LandingPageItems",
                newName: "VideoUrl");

            migrationBuilder.RenameColumn(
                name: "TextLandingPageItem_FontColor",
                table: "LandingPageItems",
                newName: "FontColor");

            migrationBuilder.RenameColumn(
                name: "TextLandingPageItem_Content",
                table: "LandingPageItems",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "TextLandingPageItem_BackgroundColor",
                table: "LandingPageItems",
                newName: "BackgroundColor");

            migrationBuilder.RenameColumn(
                name: "ImageLandingPageItem_AltText",
                table: "LandingPageItems",
                newName: "AltText");

            migrationBuilder.RenameColumn(
                name: "ImageLandingPageItem_Image_UrlPath",
                table: "LandingPageItems",
                newName: "Image_UrlPath");

            migrationBuilder.RenameColumn(
                name: "ImageLandingPageItem_Image_FileName",
                table: "LandingPageItems",
                newName: "Image_FileName");
        }
    }
}
