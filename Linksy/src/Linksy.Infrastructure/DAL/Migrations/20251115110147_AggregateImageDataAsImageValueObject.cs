using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Linksy.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AggregateImageDataAsImageValueObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrlPath",
                table: "QrCodes",
                newName: "ScanCodeImage_UrlPath");

            migrationBuilder.RenameColumn(
                name: "LogoImageUrlPath",
                table: "LaningPages",
                newName: "LogoImage_UrlPath");

            migrationBuilder.RenameColumn(
                name: "BackgroundImageUrl",
                table: "LaningPages",
                newName: "BackgroundImage_UrlPath");

            migrationBuilder.RenameColumn(
                name: "ImageUrlPath",
                table: "Barcodes",
                newName: "ScanCodeImage_UrlPath");

            migrationBuilder.AddColumn<string>(
                name: "ScanCodeImage_FileName",
                table: "QrCodes",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BackgroundImage_FileName",
                table: "LaningPages",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoImage_FileName",
                table: "LaningPages",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScanCodeImage_FileName",
                table: "Barcodes",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScanCodeImage_FileName",
                table: "QrCodes");

            migrationBuilder.DropColumn(
                name: "BackgroundImage_FileName",
                table: "LaningPages");

            migrationBuilder.DropColumn(
                name: "LogoImage_FileName",
                table: "LaningPages");

            migrationBuilder.DropColumn(
                name: "ScanCodeImage_FileName",
                table: "Barcodes");

            migrationBuilder.RenameColumn(
                name: "ScanCodeImage_UrlPath",
                table: "QrCodes",
                newName: "ImageUrlPath");

            migrationBuilder.RenameColumn(
                name: "LogoImage_UrlPath",
                table: "LaningPages",
                newName: "LogoImageUrlPath");

            migrationBuilder.RenameColumn(
                name: "BackgroundImage_UrlPath",
                table: "LaningPages",
                newName: "BackgroundImageUrl");

            migrationBuilder.RenameColumn(
                name: "ScanCodeImage_UrlPath",
                table: "Barcodes",
                newName: "ImageUrlPath");
        }
    }
}
