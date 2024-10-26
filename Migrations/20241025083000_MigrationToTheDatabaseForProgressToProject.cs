using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_Manager.Migrations
{
    /// <inheritdoc />
    public partial class MigrationToTheDatabaseForProgressToProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Progress",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Progress",
                table: "Projects");
        }
    }
}
