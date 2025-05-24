using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class ds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "exercise_Feedbacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseFeedback = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exercise_Feedbacks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "exerciseLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoFeedback = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    WorkOutExerciseId = table.Column<int>(type: "int", nullable: false),
                    workOutExercisesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exerciseLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exerciseLogs_workOutExercises_workOutExercisesId",
                        column: x => x.workOutExercisesId,
                        principalTable: "workOutExercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_exerciseLogs_workOutExercisesId",
                table: "exerciseLogs",
                column: "workOutExercisesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exercise_Feedbacks");

            migrationBuilder.DropTable(
                name: "exerciseLogs");
        }
    }
}
