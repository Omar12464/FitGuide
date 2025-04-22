using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class add5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userGoals_GoalTempelate_GoalTempelateId",
                table: "userGoals");

            migrationBuilder.RenameColumn(
                name: "GoalTempelateId",
                table: "userGoals",
                newName: "goalTempelateId");

            migrationBuilder.RenameIndex(
                name: "IX_userGoals_GoalTempelateId",
                table: "userGoals",
                newName: "IX_userGoals_goalTempelateId");

            migrationBuilder.AlterColumn<int>(
                name: "goalTempelateId",
                table: "userGoals",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_userGoals_GoalTempelate_goalTempelateId",
                table: "userGoals",
                column: "goalTempelateId",
                principalTable: "GoalTempelate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userGoals_GoalTempelate_goalTempelateId",
                table: "userGoals");

            migrationBuilder.RenameColumn(
                name: "goalTempelateId",
                table: "userGoals",
                newName: "GoalTempelateId");

            migrationBuilder.RenameIndex(
                name: "IX_userGoals_goalTempelateId",
                table: "userGoals",
                newName: "IX_userGoals_GoalTempelateId");

            migrationBuilder.AlterColumn<int>(
                name: "GoalTempelateId",
                table: "userGoals",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_userGoals_GoalTempelate_GoalTempelateId",
                table: "userGoals",
                column: "GoalTempelateId",
                principalTable: "GoalTempelate",
                principalColumn: "Id");
        }
    }
}
