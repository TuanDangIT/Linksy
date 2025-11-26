using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Linksy.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedOnDeleteCascadeForUmtParametersAndQrCodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QrCodes_UmtParameters_UmtParameterId",
                table: "QrCodes");

            migrationBuilder.AddForeignKey(
                name: "FK_QrCodes_UmtParameters_UmtParameterId",
                table: "QrCodes",
                column: "UmtParameterId",
                principalTable: "UmtParameters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QrCodes_UmtParameters_UmtParameterId",
                table: "QrCodes");

            migrationBuilder.AddForeignKey(
                name: "FK_QrCodes_UmtParameters_UmtParameterId",
                table: "QrCodes",
                column: "UmtParameterId",
                principalTable: "UmtParameters",
                principalColumn: "Id");
        }
    }
}
