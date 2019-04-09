using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteMonitoring.Migrations
{
    public partial class jobid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JobId",
                table: "SiteStatuses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobId",
                table: "SiteStatuses");
        }
    }
}
