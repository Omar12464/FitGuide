using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class add6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userGoals_GoalTempelate_goalTempelateId",
                table: "userGoals");

            migrationBuilder.DropIndex(
                name: "IX_userGoals_goalTempelateId",
                table: "userGoals");

            migrationBuilder.DropColumn(
                name: "GoalTemplateId",
                table: "userGoals");

            migrationBuilder.DropColumn(
                name: "goalTempelateId",
                table: "userGoals");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "userGoals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "userGoals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "targetBMI",
                table: "userGoals",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "targetMuscleMass",
                table: "userGoals",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "targetWaterMass",
                table: "userGoals",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "targetWeight",
                table: "userGoals",
                type: "real",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "userGoals");

            migrationBuilder.DropColumn(
                name: "name",
                table: "userGoals");

            migrationBuilder.DropColumn(
                name: "targetBMI",
                table: "userGoals");

            migrationBuilder.DropColumn(
                name: "targetMuscleMass",
                table: "userGoals");

            migrationBuilder.DropColumn(
                name: "targetWaterMass",
                table: "userGoals");

            migrationBuilder.DropColumn(
                name: "targetWeight",
                table: "userGoals");

            migrationBuilder.AddColumn<int>(
                name: "GoalTemplateId",
                table: "userGoals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "goalTempelateId",
                table: "userGoals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_userGoals_goalTempelateId",
                table: "userGoals",
                column: "goalTempelateId");

            migrationBuilder.AddForeignKey(
                name: "FK_userGoals_GoalTempelate_goalTempelateId",
                table: "userGoals",
                column: "goalTempelateId",
                principalTable: "GoalTempelate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
