using Microsoft.EntityFrameworkCore.Migrations;

namespace askaway.Migrations
{
    public partial class questionEditMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasStarredAnswer",
                table: "Questions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasStarredAnswer",
                table: "Questions");
        }
    }
}
