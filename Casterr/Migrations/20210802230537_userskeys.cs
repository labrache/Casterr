using Microsoft.EntityFrameworkCore.Migrations;

namespace Casterr.Migrations
{
    public partial class userskeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "mailKey",
                table: "AspNetUsers",
                type: "TEXT",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "mailSubscribe",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "mailKey",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "mailSubscribe",
                table: "AspNetUsers");
        }
    }
}
