using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_Manager.Migrations
{
    /// <inheritdoc />
    public partial class AddChildTaskIdTrial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ChildTaskId",
                table: "Task",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TaskId",
                table: "Task",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GroupUser",
                columns: table => new
                {
                    GroupsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupUser", x => new { x.GroupsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_GroupUser_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupUser_Groups_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Task_TaskId",
                table: "Task",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupUser_UsersId",
                table: "GroupUser",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Task_TaskId",
                table: "Task",
                column: "TaskId",
                principalTable: "Task",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Task_Task_TaskId",
                table: "Task");

            migrationBuilder.DropTable(
                name: "GroupUser");

            migrationBuilder.DropIndex(
                name: "IX_Task_TaskId",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "ChildTaskId",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "Task");
        }
    }
}
