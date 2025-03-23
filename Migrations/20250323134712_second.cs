using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitGuideTest.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealFoods_Meals_MealId1",
                table: "MealFoods");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutPlanExercises_WorkoutPlans_WorkoutPlanId1",
                table: "WorkoutPlanExercises");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkoutPlanExercises",
                table: "WorkoutPlanExercises");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutPlanExercises_WorkoutPlanId1",
                table: "WorkoutPlanExercises");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MealFoods",
                table: "MealFoods");

            migrationBuilder.DropIndex(
                name: "IX_MealFoods_MealId1",
                table: "MealFoods");

            migrationBuilder.DropColumn(
                name: "WorkoutPlanId1",
                table: "WorkoutPlanExercises");

            migrationBuilder.DropColumn(
                name: "MealId1",
                table: "MealFoods");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "WorkoutPlans",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "WorkoutPlans",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "WorkoutPlanId",
                table: "WorkoutPlanExercises",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsAchieved",
                table: "UserGoals",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "NutritionPlans",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MealName",
                table: "Meals",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "MealId",
                table: "MealFoods",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Foods",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Foods",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "TypeOfMachine",
                table: "Exercises",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "TargetMuscle",
                table: "Exercises",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Exercises",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Exercises",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkoutPlanExercises",
                table: "WorkoutPlanExercises",
                columns: new[] { "WorkoutPlanId", "ExerciseId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MealFoods",
                table: "MealFoods",
                columns: new[] { "MealId", "FoodId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MealFoods_Meals_MealId",
                table: "MealFoods",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutPlanExercises_WorkoutPlans_WorkoutPlanId",
                table: "WorkoutPlanExercises",
                column: "WorkoutPlanId",
                principalTable: "WorkoutPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealFoods_Meals_MealId",
                table: "MealFoods");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutPlanExercises_WorkoutPlans_WorkoutPlanId",
                table: "WorkoutPlanExercises");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkoutPlanExercises",
                table: "WorkoutPlanExercises");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MealFoods",
                table: "MealFoods");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "WorkoutPlans",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "WorkoutPlans",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<int>(
                name: "WorkoutPlanId",
                table: "WorkoutPlanExercises",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "WorkoutPlanId1",
                table: "WorkoutPlanExercises",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<bool>(
                name: "IsAchieved",
                table: "UserGoals",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "NutritionPlans",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "MealName",
                table: "Meals",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "MealId",
                table: "MealFoods",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "MealId1",
                table: "MealFoods",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Foods",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Foods",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "TypeOfMachine",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "TargetMuscle",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkoutPlanExercises",
                table: "WorkoutPlanExercises",
                column: "WorkoutPlanId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MealFoods",
                table: "MealFoods",
                column: "MealId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutPlanExercises_WorkoutPlanId1",
                table: "WorkoutPlanExercises",
                column: "WorkoutPlanId1");

            migrationBuilder.CreateIndex(
                name: "IX_MealFoods_MealId1",
                table: "MealFoods",
                column: "MealId1");

            migrationBuilder.AddForeignKey(
                name: "FK_MealFoods_Meals_MealId1",
                table: "MealFoods",
                column: "MealId1",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutPlanExercises_WorkoutPlans_WorkoutPlanId1",
                table: "WorkoutPlanExercises",
                column: "WorkoutPlanId1",
                principalTable: "WorkoutPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
