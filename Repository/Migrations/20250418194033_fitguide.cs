using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class fitguide : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BMI",
                table: "GoalTempelate");

            migrationBuilder.DropColumn(
                name: "MuscleMass",
                table: "GoalTempelate");

            migrationBuilder.DropColumn(
                name: "WaterMass",
                table: "GoalTempelate");

            migrationBuilder.DropColumn(
                name: "weights",
                table: "GoalTempelate");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "GoalTempelate",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "TypeName",
                table: "GoalTempelate",
                newName: "name");

            migrationBuilder.AddColumn<float>(
                name: "targetBMI",
                table: "GoalTempelate",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "targetMuscleMass",
                table: "GoalTempelate",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "targetWaterMass",
                table: "GoalTempelate",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "targetWeight",
                table: "GoalTempelate",
                type: "real",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "targetBMI",
                table: "GoalTempelate");

            migrationBuilder.DropColumn(
                name: "targetMuscleMass",
                table: "GoalTempelate");

            migrationBuilder.DropColumn(
                name: "targetWaterMass",
                table: "GoalTempelate");

            migrationBuilder.DropColumn(
                name: "targetWeight",
                table: "GoalTempelate");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "GoalTempelate",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "GoalTempelate",
                newName: "TypeName");

            migrationBuilder.AddColumn<float>(
                name: "BMI",
                table: "GoalTempelate",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "MuscleMass",
                table: "GoalTempelate",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "WaterMass",
                table: "GoalTempelate",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "weights",
                table: "GoalTempelate",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
