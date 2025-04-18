using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class Goalt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userGoals_goalTempelates_GoalTempelateId",
                table: "userGoals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_goalTempelates",
                table: "goalTempelates");

            migrationBuilder.RenameTable(
                name: "goalTempelates",
                newName: "GoalTempelate");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GoalTempelate",
                table: "GoalTempelate",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_userGoals_GoalTempelate_GoalTempelateId",
                table: "userGoals",
                column: "GoalTempelateId",
                principalTable: "GoalTempelate",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userGoals_GoalTempelate_GoalTempelateId",
                table: "userGoals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GoalTempelate",
                table: "GoalTempelate");

            migrationBuilder.RenameTable(
                name: "GoalTempelate",
                newName: "goalTempelates");

            migrationBuilder.AddPrimaryKey(
                name: "PK_goalTempelates",
                table: "goalTempelates",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_userGoals_goalTempelates_GoalTempelateId",
                table: "userGoals",
                column: "GoalTempelateId",
                principalTable: "goalTempelates",
                principalColumn: "Id");
        }
    }
}
