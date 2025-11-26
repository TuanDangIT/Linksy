using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Linksy.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserIdToUmtParameters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UmtParameters",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UmtParameters");
        }
    }
}
