using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectHub.Migrations
{
    public partial class InactiveAddedOnApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Inactive",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Inactive",
                table: "AspNetUsers");
        }
    }
}
