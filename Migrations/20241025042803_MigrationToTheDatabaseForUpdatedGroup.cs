using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_Manager.Migrations
{
    /// <inheritdoc />
    public partial class MigrationToTheDatabaseForUpdatedGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Groups_GroupId",
                table: "Projects");

            migrationBuilder.AlterColumn<Guid>(
                name: "GroupId",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Groups_GroupId",
                table: "Projects",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Groups_GroupId",
                table: "Projects");

            migrationBuilder.AlterColumn<Guid>(
                name: "GroupId",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Groups_GroupId",
                table: "Projects",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
