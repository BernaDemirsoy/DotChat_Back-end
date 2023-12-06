using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotChat_Repositories.Migrations
{
    public partial class v10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "chatGroups",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "chatGroupMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "chatGroupMembers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ChatGroupMemberInboxes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ChatConnectionLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "chatGroups");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "chatGroupMessages");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "chatGroupMembers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ChatGroupMemberInboxes");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ChatConnectionLogs");
        }
    }
}
