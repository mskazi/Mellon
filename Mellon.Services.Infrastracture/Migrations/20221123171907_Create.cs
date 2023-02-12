using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mellon.Services.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class Create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApprovalOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ERPCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ERPCompany = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Bu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BUName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BLName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PLLines = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PLLinesName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ergo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErgoName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ERPTimeStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotificationMail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApprovalLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ERPCompany = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocumentNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LineNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LineAmount = table.Column<double>(type: "float", nullable: true),
                    BU = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BUName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BLName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PLLines = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PLLinesName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ergo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErgoName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitPrice = table.Column<double>(type: "float", nullable: true),
                    Quantity = table.Column<double>(type: "float", nullable: true),
                    ERPTimeStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    ApprovalOrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovalLines_ApprovalOrders_ApprovalOrderId",
                        column: x => x.ApprovalOrderId,
                        principalTable: "ApprovalOrders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Approvals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ERPCountry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ERPCompany = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocumentNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovalSequence = table.Column<int>(type: "int", nullable: false),
                    DocumentOwner = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestNo = table.Column<int>(type: "int", nullable: false),
                    ApprovalResponsible = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocumentOwnerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DocumentToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ERPTimeStamp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovalProcessComment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovalRequestComment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    ApprovalOrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Approvals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Approvals_ApprovalOrders_ApprovalOrderId",
                        column: x => x.ApprovalOrderId,
                        principalTable: "ApprovalOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalLines_ApprovalOrderId",
                table: "ApprovalLines",
                column: "ApprovalOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Approvals_ApprovalOrderId",
                table: "Approvals",
                column: "ApprovalOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApprovalLines");

            migrationBuilder.DropTable(
                name: "Approvals");

            migrationBuilder.DropTable(
                name: "ApprovalOrders");
        }
    }
}
