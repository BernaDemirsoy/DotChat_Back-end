using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotChat_Repositories.Migrations
{
    public partial class v15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "groupAvatarImage",
                table: "chatGroups",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "groupAvatarImage",
                table: "chatGroups");
        }
    }
}
