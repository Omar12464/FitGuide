using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class dsdsdss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_exerciseLogs_workOutExercises_workOutExercisesId",
                table: "exerciseLogs");

            migrationBuilder.DropIndex(
                name: "IX_exerciseLogs_workOutExercisesId",
                table: "exerciseLogs");

            migrationBuilder.RenameColumn(
                name: "workOutExercisesId",
                table: "exerciseLogs",
                newName: "Exercise_FeedbackId");

            migrationBuilder.AddColumn<DateTime>(
                name: "LoggedAt",
                table: "exerciseLogs",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ExerciseLogId",
                table: "exercise_Feedbacks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_exerciseLogs_WorkOutExerciseId",
                table: "exerciseLogs",
                column: "WorkOutExerciseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exercise_Feedbacks_ExerciseLogId",
                table: "exercise_Feedbacks",
                column: "ExerciseLogId");

            migrationBuilder.AddForeignKey(
                name: "FK_exercise_Feedbacks_exerciseLogs_ExerciseLogId",
                table: "exercise_Feedbacks",
                column: "ExerciseLogId",
                principalTable: "exerciseLogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_exerciseLogs_workOutExercises_WorkOutExerciseId",
                table: "exerciseLogs",
                column: "WorkOutExerciseId",
                principalTable: "workOutExercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_exercise_Feedbacks_exerciseLogs_ExerciseLogId",
                table: "exercise_Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_exerciseLogs_workOutExercises_WorkOutExerciseId",
                table: "exerciseLogs");

            migrationBuilder.DropIndex(
                name: "IX_exerciseLogs_WorkOutExerciseId",
                table: "exerciseLogs");

            migrationBuilder.DropIndex(
                name: "IX_exercise_Feedbacks_ExerciseLogId",
                table: "exercise_Feedbacks");

            migrationBuilder.DropColumn(
                name: "LoggedAt",
                table: "exerciseLogs");

            migrationBuilder.DropColumn(
                name: "ExerciseLogId",
                table: "exercise_Feedbacks");

            migrationBuilder.RenameColumn(
                name: "Exercise_FeedbackId",
                table: "exerciseLogs",
                newName: "workOutExercisesId");

            migrationBuilder.CreateIndex(
                name: "IX_exerciseLogs_workOutExercisesId",
                table: "exerciseLogs",
                column: "workOutExercisesId");

            migrationBuilder.AddForeignKey(
                name: "FK_exerciseLogs_workOutExercises_workOutExercisesId",
                table: "exerciseLogs",
                column: "workOutExercisesId",
                principalTable: "workOutExercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
