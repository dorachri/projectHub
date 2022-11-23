using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectHub.Migrations
{
    public partial class StatusLogAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StatusLogs",
                columns: table => new
                {
                    StatusLogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventDate = table.Column<DateTime>(nullable: false),
                    ProjectCode = table.Column<string>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusLogs", x => x.StatusLogId);
                    table.ForeignKey(
                        name: "FK_StatusLogs_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StatusLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StatusLogs_ProjectId",
                table: "StatusLogs",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusLogs_UserId",
                table: "StatusLogs",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatusLogs");
        }
    }
}
