using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_Manager.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTheIssueModelParentWithSubIssueToTheDatabaseAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IssueId",
                table: "Issues");

            migrationBuilder.AddColumn<Guid>(
                name: "SubIssueId",
                table: "Issues",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Issues_SubIssueId",
                table: "Issues",
                column: "SubIssueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Issues_SubIssueId",
                table: "Issues",
                column: "SubIssueId",
                principalTable: "Issues",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Issues_SubIssueId",
                table: "Issues");

            migrationBuilder.DropIndex(
                name: "IX_Issues_SubIssueId",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "SubIssueId",
                table: "Issues");

            migrationBuilder.AddColumn<Guid>(
                name: "IssueId",
                table: "Issues",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
