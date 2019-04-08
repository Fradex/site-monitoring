using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteMonitoring.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sites",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiteStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SiteId = table.Column<Guid>(nullable: false),
                    CheckedInterval = table.Column<int>(nullable: false, defaultValue: 5),
                    IsAvailable = table.Column<bool>(nullable: false,defaultValue:false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiteStatuses_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SiteStatuses_SiteId",
                table: "SiteStatuses",
                column: "SiteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SiteStatuses");

            migrationBuilder.DropTable(
                name: "Sites");
        }
    }
}
