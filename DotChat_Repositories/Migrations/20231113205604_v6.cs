using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotChat_Repositories.Migrations
{
    public partial class v6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "chatGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    connectionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isChannelClosed = table.Column<bool>(type: "bit", nullable: false),
                    channelCloseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    channelCloseUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chatGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_chatGroups_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "chatGroupMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    chatgroupId = table.Column<int>(type: "int", nullable: false),
                    memberUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    isMemberChannelAdmin = table.Column<bool>(type: "bit", nullable: false),
                    isChannelMutedByMember = table.Column<bool>(type: "bit", nullable: false),
                    isMemberRemovedFromChannel = table.Column<bool>(type: "bit", nullable: false),
                    memberRemovedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chatGroupMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_chatGroupMembers_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_chatGroupMembers_chatGroups_chatgroupId",
                        column: x => x.chatgroupId,
                        principalTable: "chatGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "chatGroupMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    chatGroupMemberId = table.Column<int>(type: "int", nullable: false),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    messageTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chatGroupMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_chatGroupMessages_chatGroupMembers_chatGroupMemberId",
                        column: x => x.chatGroupMemberId,
                        principalTable: "chatGroupMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatGroupMemberInboxes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    chatGroupMemberId = table.Column<int>(type: "int", nullable: false),
                    chatGroupMessagesId = table.Column<int>(type: "int", nullable: false),
                    isRead = table.Column<bool>(type: "bit", nullable: false),
                    readDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    isArchived = table.Column<bool>(type: "bit", nullable: false),
                    archiveDate = table.Column<DateTime>(type: "datetime2", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_chatGroupMembers_chatgroupId",
                table: "chatGroupMembers",
                column: "chatgroupId");

            migrationBuilder.CreateIndex(
                name: "IX_chatGroupMembers_userId",
                table: "chatGroupMembers",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_chatGroupMessages_chatGroupMemberId",
                table: "chatGroupMessages",
                column: "chatGroupMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_chatGroups_userId",
                table: "chatGroups",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatGroupMemberInboxes");

            migrationBuilder.DropTable(
                name: "chatGroupMessages");

            migrationBuilder.DropTable(
                name: "chatGroupMembers");

            migrationBuilder.DropTable(
                name: "chatGroups");
        }
    }
}
