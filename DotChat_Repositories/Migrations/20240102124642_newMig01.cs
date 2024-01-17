using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotChat_Repositories.Migrations
{
    public partial class newMig01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsBinaryGroup",
                table: "chatGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBinaryGroup",
                table: "chatGroups");
        }
    }
}
