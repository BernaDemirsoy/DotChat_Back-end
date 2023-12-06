using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotChat_Repositories.Migrations
{
    public partial class v8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatConnectionLog_AspNetUsers_userId",
                table: "ChatConnectionLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatConnectionLog",
                table: "ChatConnectionLog");

            migrationBuilder.RenameTable(
                name: "ChatConnectionLog",
                newName: "ChatConnectionLogs");

            migrationBuilder.RenameIndex(
                name: "IX_ChatConnectionLog_userId",
                table: "ChatConnectionLogs",
                newName: "IX_ChatConnectionLogs_userId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatConnectionLogs",
                table: "ChatConnectionLogs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatConnectionLogs_AspNetUsers_userId",
                table: "ChatConnectionLogs",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatConnectionLogs_AspNetUsers_userId",
                table: "ChatConnectionLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatConnectionLogs",
                table: "ChatConnectionLogs");

            migrationBuilder.RenameTable(
                name: "ChatConnectionLogs",
                newName: "ChatConnectionLog");

            migrationBuilder.RenameIndex(
                name: "IX_ChatConnectionLogs_userId",
                table: "ChatConnectionLog",
                newName: "IX_ChatConnectionLog_userId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatConnectionLog",
                table: "ChatConnectionLog",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatConnectionLog_AspNetUsers_userId",
                table: "ChatConnectionLog",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
