using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotChat_Repositories.Migrations
{
    public partial class newMig11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "deliveredDate",
                table: "ChatGroupMemberInboxes",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deliveredDate",
                table: "ChatGroupMemberInboxes");
        }
    }
}
