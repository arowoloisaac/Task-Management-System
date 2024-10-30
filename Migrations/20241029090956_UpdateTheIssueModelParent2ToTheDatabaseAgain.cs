using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_Manager.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTheIssueModelParent2ToTheDatabaseAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Issues_SubIssueId",
                table: "Issues");

            migrationBuilder.RenameColumn(
                name: "SubIssueId",
                table: "Issues",
                newName: "ParentIssueId");

            migrationBuilder.RenameIndex(
                name: "IX_Issues_SubIssueId",
                table: "Issues",
                newName: "IX_Issues_ParentIssueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Issues_ParentIssueId",
                table: "Issues",
                column: "ParentIssueId",
                principalTable: "Issues",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Issues_ParentIssueId",
                table: "Issues");

            migrationBuilder.RenameColumn(
                name: "ParentIssueId",
                table: "Issues",
                newName: "SubIssueId");

            migrationBuilder.RenameIndex(
                name: "IX_Issues_ParentIssueId",
                table: "Issues",
                newName: "IX_Issues_SubIssueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Issues_SubIssueId",
                table: "Issues",
                column: "SubIssueId",
                principalTable: "Issues",
                principalColumn: "Id");
        }
    }
}
