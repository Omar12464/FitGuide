using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class Goal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BMI",
                table: "userGoals");

            migrationBuilder.DropColumn(
                name: "MuscleMass",
                table: "userGoals");

            migrationBuilder.DropColumn(
                name: "TypeName",
                table: "userGoals");

            migrationBuilder.DropColumn(
                name: "WaterMass",
                table: "userGoals");

            migrationBuilder.DropColumn(
                name: "weights",
                table: "userGoals");

            migrationBuilder.AddColumn<int>(
                name: "GoalTempelateId",
                table: "userGoals",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GoalTemplateId",
                table: "userGoals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "goalTempelates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BMI = table.Column<float>(type: "real", nullable: false),
                    weights = table.Column<float>(type: "real", nullable: false),
                    MuscleMass = table.Column<float>(type: "real", nullable: false),
                    WaterMass = table.Column<float>(type: "real", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_goalTempelates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_userGoals_GoalTempelateId",
                table: "userGoals",
                column: "GoalTempelateId");

            migrationBuilder.AddForeignKey(
                name: "FK_userGoals_goalTempelates_GoalTempelateId",
                table: "userGoals",
                column: "GoalTempelateId",
                principalTable: "goalTempelates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userGoals_goalTempelates_GoalTempelateId",
                table: "userGoals");

            migrationBuilder.DropTable(
                name: "goalTempelates");

            migrationBuilder.DropIndex(
                name: "IX_userGoals_GoalTempelateId",
                table: "userGoals");

            migrationBuilder.DropColumn(
                name: "GoalTempelateId",
                table: "userGoals");

            migrationBuilder.DropColumn(
                name: "GoalTemplateId",
                table: "userGoals");

            migrationBuilder.AddColumn<float>(
                name: "BMI",
                table: "userGoals",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "MuscleMass",
                table: "userGoals",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeName",
                table: "userGoals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "WaterMass",
                table: "userGoals",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "weights",
                table: "userGoals",
                type: "real",
                nullable: true);
        }
    }
}
