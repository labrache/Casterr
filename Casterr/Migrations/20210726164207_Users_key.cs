using Microsoft.EntityFrameworkCore.Migrations;

namespace Casterr.Migrations
{
    public partial class Users_key : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "uKey",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "uKey",
                table: "AspNetUsers");
        }
    }
}
