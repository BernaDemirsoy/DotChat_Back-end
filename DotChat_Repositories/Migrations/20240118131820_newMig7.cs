using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotChat_Repositories.Migrations
{
    public partial class newMig7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "chatGroupMemberId",
                table: "ChatGroupMemberInboxes",
                newName: "chatGroupId");

            migrationBuilder.AddColumn<int>(
                name: "TochatGroupMemberId",
                table: "chatGroupMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TochatGroupMemberId",
                table: "ChatGroupMemberInboxes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TochatGroupMemberId",
                table: "chatGroupMessages");

            migrationBuilder.DropColumn(
                name: "TochatGroupMemberId",
                table: "ChatGroupMemberInboxes");

            migrationBuilder.RenameColumn(
                name: "chatGroupId",
                table: "ChatGroupMemberInboxes",
                newName: "chatGroupMemberId");
        }
    }
}
