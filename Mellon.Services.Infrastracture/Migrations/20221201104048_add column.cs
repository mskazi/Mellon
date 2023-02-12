using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mellon.Services.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class addcolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocumentOwner",
                table: "ApprovalNotifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentOwner",
                table: "ApprovalNotifications");
        }
    }
}
