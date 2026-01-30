using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROCTORSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddConnectionIdToStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConnectionId",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectionId",
                table: "Students");
        }
    }
}
