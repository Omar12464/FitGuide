using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddJson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GifBytes",
                table: "Exercise");

            migrationBuilder.DropColumn(
                name: "GifPath",
                table: "Exercise");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "WorkOutPlans",
                newName: "DifficultyLevel");

            migrationBuilder.RenameColumn(
                name: "InjuryId",
                table: "userInjuries",
                newName: "injuryId");

            migrationBuilder.RenameIndex(
                name: "IX_userInjuries_UserId_InjuryId",
                table: "userInjuries",
                newName: "IX_userInjuries_UserId_injuryId");

            migrationBuilder.AddColumn<string>(
                name: "fitnessLevel",
                table: "userMetrics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "injuryId",
                table: "userInjuries",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "AffectedBodyPart",
                table: "Injury",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContraindicatedExercises",
                table: "Injury",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "SuitableEquipment",
                table: "Injury",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "SuitableExercises",
                table: "Injury",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AlterColumn<int>(
                name: "Difficulty",
                table: "Exercise",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "TargetInjury",
                table: "Exercise",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.CreateTable(
                name: "workOutExercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkoOutId = table.Column<int>(type: "int", nullable: false),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    NumberOfSets = table.Column<int>(type: "int", nullable: false),
                    NumberOfReps = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workOutExercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_workOutExercises_Exercise_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_workOutExercises_WorkOutPlans_WorkoOutId",
                        column: x => x.WorkoOutId,
                        principalTable: "WorkOutPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_userInjuries_injuryId",
                table: "userInjuries",
                column: "injuryId");

            migrationBuilder.CreateIndex(
                name: "IX_workOutExercises_ExerciseId",
                table: "workOutExercises",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_workOutExercises_WorkoOutId",
                table: "workOutExercises",
                column: "WorkoOutId");

            migrationBuilder.AddForeignKey(
                name: "FK_userInjuries_Injury_injuryId",
                table: "userInjuries",
                column: "injuryId",
                principalTable: "Injury",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userInjuries_Injury_injuryId",
                table: "userInjuries");

            migrationBuilder.DropTable(
                name: "workOutExercises");

            migrationBuilder.DropIndex(
                name: "IX_userInjuries_injuryId",
                table: "userInjuries");

            migrationBuilder.DropColumn(
                name: "fitnessLevel",
                table: "userMetrics");

            migrationBuilder.DropColumn(
                name: "AffectedBodyPart",
                table: "Injury");

            migrationBuilder.DropColumn(
                name: "ContraindicatedExercises",
                table: "Injury");

            migrationBuilder.DropColumn(
                name: "SuitableEquipment",
                table: "Injury");

            migrationBuilder.DropColumn(
                name: "SuitableExercises",
                table: "Injury");

            migrationBuilder.DropColumn(
                name: "TargetInjury",
                table: "Exercise");

            migrationBuilder.RenameColumn(
                name: "DifficultyLevel",
                table: "WorkOutPlans",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "injuryId",
                table: "userInjuries",
                newName: "InjuryId");

            migrationBuilder.RenameIndex(
                name: "IX_userInjuries_UserId_injuryId",
                table: "userInjuries",
                newName: "IX_userInjuries_UserId_InjuryId");

            migrationBuilder.AlterColumn<string>(
                name: "InjuryId",
                table: "userInjuries",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Difficulty",
                table: "Exercise",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<byte[]>(
                name: "GifBytes",
                table: "Exercise",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "GifPath",
                table: "Exercise",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
