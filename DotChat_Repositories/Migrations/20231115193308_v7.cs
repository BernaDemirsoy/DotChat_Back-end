using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotChat_Repositories.Migrations
{
    public partial class v7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "connectionId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "ChatConnectionLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    connectionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    connectedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    disConnectedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatConnectionLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatConnectionLog_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatConnectionLog_userId",
                table: "ChatConnectionLog",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatConnectionLog");

            migrationBuilder.AddColumn<string>(
                name: "connectionId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
