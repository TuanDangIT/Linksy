using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Linksy.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddOneToManyRelationshipBetweenUmtParameterAndQrCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QrCodeId",
                table: "UmtParameter",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UmtParameter_QrCodeId",
                table: "UmtParameter",
                column: "QrCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_UmtParameter_QrCodes_QrCodeId",
                table: "UmtParameter",
                column: "QrCodeId",
                principalTable: "QrCodes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UmtParameter_QrCodes_QrCodeId",
                table: "UmtParameter");

            migrationBuilder.DropIndex(
                name: "IX_UmtParameter_QrCodeId",
                table: "UmtParameter");

            migrationBuilder.DropColumn(
                name: "QrCodeId",
                table: "UmtParameter");
        }
    }
}
