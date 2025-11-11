using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Linksy.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class DeletedTablePerTypeConfigForLandingPageItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageLandingPageItem");

            migrationBuilder.DropTable(
                name: "TextLandingPageItems");

            migrationBuilder.DropTable(
                name: "UrlLandingPageItems");

            migrationBuilder.DropTable(
                name: "YoutubeLandingPageItems");

            migrationBuilder.AddColumn<string>(
                name: "AltText",
                table: "LandingPageItems",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BackgroundColor",
                table: "LandingPageItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "LandingPageItems",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FontColor",
                table: "LandingPageItems",
                type: "character varying(16)",
                maxLength: 16,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrlPath",
                table: "LandingPageItems",
                type: "character varying(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UrlId",
                table: "LandingPageItems",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlLandingPageItem_BackgroundColor",
                table: "LandingPageItems",
                type: "character varying(16)",
                maxLength: 16,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlLandingPageItem_Content",
                table: "LandingPageItems",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlLandingPageItem_FontColor",
                table: "LandingPageItems",
                type: "character varying(16)",
                maxLength: 16,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UrlLandingPageItem_UrlId",
                table: "LandingPageItems",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "LandingPageItems",
                type: "character varying(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LandingPageItems_UrlId",
                table: "LandingPageItems",
                column: "UrlId");

            migrationBuilder.CreateIndex(
                name: "IX_LandingPageItems_UrlLandingPageItem_UrlId",
                table: "LandingPageItems",
                column: "UrlLandingPageItem_UrlId");

            migrationBuilder.AddForeignKey(
                name: "FK_LandingPageItems_Urls_UrlId",
                table: "LandingPageItems",
                column: "UrlId",
                principalTable: "Urls",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LandingPageItems_Urls_UrlLandingPageItem_UrlId",
                table: "LandingPageItems",
                column: "UrlLandingPageItem_UrlId",
                principalTable: "Urls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LandingPageItems_Urls_UrlId",
                table: "LandingPageItems");

            migrationBuilder.DropForeignKey(
                name: "FK_LandingPageItems_Urls_UrlLandingPageItem_UrlId",
                table: "LandingPageItems");

            migrationBuilder.DropIndex(
                name: "IX_LandingPageItems_UrlId",
                table: "LandingPageItems");

            migrationBuilder.DropIndex(
                name: "IX_LandingPageItems_UrlLandingPageItem_UrlId",
                table: "LandingPageItems");

            migrationBuilder.DropColumn(
                name: "AltText",
                table: "LandingPageItems");

            migrationBuilder.DropColumn(
                name: "BackgroundColor",
                table: "LandingPageItems");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "LandingPageItems");

            migrationBuilder.DropColumn(
                name: "FontColor",
                table: "LandingPageItems");

            migrationBuilder.DropColumn(
                name: "ImageUrlPath",
                table: "LandingPageItems");

            migrationBuilder.DropColumn(
                name: "UrlId",
                table: "LandingPageItems");

            migrationBuilder.DropColumn(
                name: "UrlLandingPageItem_BackgroundColor",
                table: "LandingPageItems");

            migrationBuilder.DropColumn(
                name: "UrlLandingPageItem_Content",
                table: "LandingPageItems");

            migrationBuilder.DropColumn(
                name: "UrlLandingPageItem_FontColor",
                table: "LandingPageItems");

            migrationBuilder.DropColumn(
                name: "UrlLandingPageItem_UrlId",
                table: "LandingPageItems");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "LandingPageItems");

            migrationBuilder.CreateTable(
                name: "ImageLandingPageItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    UrlId = table.Column<int>(type: "integer", nullable: true),
                    AltText = table.Column<string>(type: "text", nullable: false),
                    ImageUrlPath = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageLandingPageItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageLandingPageItem_Urls_UrlId",
                        column: x => x.UrlId,
                        principalTable: "Urls",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TextLandingPageItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    BackgroundColor = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    FontColor = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextLandingPageItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TextLandingPageItems_LandingPageItems_Id",
                        column: x => x.Id,
                        principalTable: "LandingPageItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UrlLandingPageItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    UrlId = table.Column<int>(type: "integer", nullable: false),
                    BackgroundColor = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Content = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    FontColor = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlLandingPageItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrlLandingPageItems_LandingPageItems_Id",
                        column: x => x.Id,
                        principalTable: "LandingPageItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UrlLandingPageItems_Urls_UrlId",
                        column: x => x.UrlId,
                        principalTable: "Urls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "YoutubeLandingPageItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    VideoUrl = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YoutubeLandingPageItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YoutubeLandingPageItems_LandingPageItems_Id",
                        column: x => x.Id,
                        principalTable: "LandingPageItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageLandingPageItem_UrlId",
                table: "ImageLandingPageItem",
                column: "UrlId");

            migrationBuilder.CreateIndex(
                name: "IX_UrlLandingPageItems_UrlId",
                table: "UrlLandingPageItems",
                column: "UrlId");
        }
    }
}
