using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Linksy.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CorrectedLandingPageTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LadingPageEngagements_LaningPages_LandingPageId",
                table: "LadingPageEngagements");

            migrationBuilder.DropForeignKey(
                name: "FK_LandingPageItems_LaningPages_LandingPageId",
                table: "LandingPageItems");

            migrationBuilder.DropForeignKey(
                name: "FK_LandingPageViews_LaningPages_LandingPageId",
                table: "LandingPageViews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LaningPages",
                table: "LaningPages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LadingPageEngagements",
                table: "LadingPageEngagements");

            migrationBuilder.RenameTable(
                name: "LaningPages",
                newName: "LandingPages");

            migrationBuilder.RenameTable(
                name: "LadingPageEngagements",
                newName: "LandingPageEngagements");

            migrationBuilder.RenameIndex(
                name: "IX_LaningPages_Code",
                table: "LandingPages",
                newName: "IX_LandingPages_Code");

            migrationBuilder.RenameIndex(
                name: "IX_LadingPageEngagements_LandingPageId",
                table: "LandingPageEngagements",
                newName: "IX_LandingPageEngagements_LandingPageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LandingPages",
                table: "LandingPages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LandingPageEngagements",
                table: "LandingPageEngagements",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LandingPageEngagements_LandingPages_LandingPageId",
                table: "LandingPageEngagements",
                column: "LandingPageId",
                principalTable: "LandingPages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LandingPageItems_LandingPages_LandingPageId",
                table: "LandingPageItems",
                column: "LandingPageId",
                principalTable: "LandingPages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LandingPageViews_LandingPages_LandingPageId",
                table: "LandingPageViews",
                column: "LandingPageId",
                principalTable: "LandingPages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LandingPageEngagements_LandingPages_LandingPageId",
                table: "LandingPageEngagements");

            migrationBuilder.DropForeignKey(
                name: "FK_LandingPageItems_LandingPages_LandingPageId",
                table: "LandingPageItems");

            migrationBuilder.DropForeignKey(
                name: "FK_LandingPageViews_LandingPages_LandingPageId",
                table: "LandingPageViews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LandingPages",
                table: "LandingPages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LandingPageEngagements",
                table: "LandingPageEngagements");

            migrationBuilder.RenameTable(
                name: "LandingPages",
                newName: "LaningPages");

            migrationBuilder.RenameTable(
                name: "LandingPageEngagements",
                newName: "LadingPageEngagements");

            migrationBuilder.RenameIndex(
                name: "IX_LandingPages_Code",
                table: "LaningPages",
                newName: "IX_LaningPages_Code");

            migrationBuilder.RenameIndex(
                name: "IX_LandingPageEngagements_LandingPageId",
                table: "LadingPageEngagements",
                newName: "IX_LadingPageEngagements_LandingPageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LaningPages",
                table: "LaningPages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LadingPageEngagements",
                table: "LadingPageEngagements",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LadingPageEngagements_LaningPages_LandingPageId",
                table: "LadingPageEngagements",
                column: "LandingPageId",
                principalTable: "LaningPages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LandingPageItems_LaningPages_LandingPageId",
                table: "LandingPageItems",
                column: "LandingPageId",
                principalTable: "LaningPages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LandingPageViews_LaningPages_LandingPageId",
                table: "LandingPageViews",
                column: "LandingPageId",
                principalTable: "LaningPages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
