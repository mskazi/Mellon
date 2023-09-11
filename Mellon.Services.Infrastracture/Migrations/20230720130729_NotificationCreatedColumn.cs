using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mellon.Services.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class NotificationCreatedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "NotificationCreated",
                table: "ApprovalNotifications",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationCreated",
                table: "ApprovalNotifications");
        }
    }
}
