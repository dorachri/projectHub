using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectHub.Migrations
{
    public partial class ChangeStatusLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NewStatus",
                table: "StatusLogs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OldStatus",
                table: "StatusLogs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewStatus",
                table: "StatusLogs");

            migrationBuilder.DropColumn(
                name: "OldStatus",
                table: "StatusLogs");
        }
    }
}
