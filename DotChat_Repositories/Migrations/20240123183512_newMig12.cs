using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotChat_Repositories.Migrations
{
    public partial class newMig12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatGroupMemberInboxes");

            migrationBuilder.DropColumn(
                name: "messageTimestamp",
                table: "chatGroupMessages");

            migrationBuilder.AddColumn<DateTime>(
                name: "archiveDate",
                table: "chatGroupMessages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "deletedDate",
                table: "chatGroupMessages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "deliveredDate",
                table: "chatGroupMessages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isArchived",
                table: "chatGroupMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isDelivered",
                table: "chatGroupMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isRead",
                table: "chatGroupMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "readDate",
                table: "chatGroupMessages",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "archiveDate",
                table: "chatGroupMessages");

            migrationBuilder.DropColumn(
                name: "deletedDate",
                table: "chatGroupMessages");

            migrationBuilder.DropColumn(
                name: "deliveredDate",
                table: "chatGroupMessages");

            migrationBuilder.DropColumn(
                name: "isArchived",
                table: "chatGroupMessages");

            migrationBuilder.DropColumn(
                name: "isDelivered",
                table: "chatGroupMessages");

            migrationBuilder.DropColumn(
                name: "isRead",
                table: "chatGroupMessages");

            migrationBuilder.DropColumn(
                name: "readDate",
                table: "chatGroupMessages");

            migrationBuilder.AddColumn<DateTime>(
                name: "messageTimestamp",
                table: "chatGroupMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "ChatGroupMemberInboxes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    chatGroupMessagesId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TochatGroupMemberId = table.Column<int>(type: "int", nullable: false),
                    archiveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    chatGroupId = table.Column<int>(type: "int", nullable: false),
                    deliveredDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    isArchived = table.Column<bool>(type: "bit", nullable: false),
                    isDelivered = table.Column<bool>(type: "bit", nullable: false),
                    isRead = table.Column<bool>(type: "bit", nullable: false),
                    readDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    userId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatGroupMemberInboxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatGroupMemberInboxes_chatGroupMessages_chatGroupMessagesId",
                        column: x => x.chatGroupMessagesId,
                        principalTable: "chatGroupMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatGroupMemberInboxes_chatGroupMessagesId",
                table: "ChatGroupMemberInboxes",
                column: "chatGroupMessagesId",
                unique: true);
        }
    }
}
