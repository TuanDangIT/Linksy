using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Linksy.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RenameUrlIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LandingPageItems_Urls_UrlId",
                table: "LandingPageItems");

            migrationBuilder.RenameColumn(
                name: "UrlId",
                table: "LandingPageItems",
                newName: "ImageLandingPageItem_UrlId");

            migrationBuilder.RenameIndex(
                name: "IX_LandingPageItems_UrlId",
                table: "LandingPageItems",
                newName: "IX_LandingPageItems_ImageLandingPageItem_UrlId");

            migrationBuilder.AddForeignKey(
                name: "FK_LandingPageItems_Urls_ImageLandingPageItem_UrlId",
                table: "LandingPageItems",
                column: "ImageLandingPageItem_UrlId",
                principalTable: "Urls",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LandingPageItems_Urls_ImageLandingPageItem_UrlId",
                table: "LandingPageItems");

            migrationBuilder.RenameColumn(
                name: "ImageLandingPageItem_UrlId",
                table: "LandingPageItems",
                newName: "UrlId");

            migrationBuilder.RenameIndex(
                name: "IX_LandingPageItems_ImageLandingPageItem_UrlId",
                table: "LandingPageItems",
                newName: "IX_LandingPageItems_UrlId");

            migrationBuilder.AddForeignKey(
                name: "FK_LandingPageItems_Urls_UrlId",
                table: "LandingPageItems",
                column: "UrlId",
                principalTable: "Urls",
                principalColumn: "Id");
        }
    }
}
