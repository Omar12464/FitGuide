using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class add2525252525 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BarCode",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "Calories",
                table: "Food");

            migrationBuilder.RenameColumn(
                name: "Protein",
                table: "Food",
                newName: "ProteinPerServing");

            migrationBuilder.RenameColumn(
                name: "Fats",
                table: "Food",
                newName: "FatPerServing");

            migrationBuilder.RenameColumn(
                name: "Carbs",
                table: "Food",
                newName: "CarbsPerServing");

            migrationBuilder.AddColumn<double>(
                name: "GymFrequency",
                table: "userMetrics",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<double>(
                name: "ProteinTarget",
                table: "nutritionPlans",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<double>(
                name: "FatTarget",
                table: "nutritionPlans",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<double>(
                name: "CarbsTarget",
                table: "nutritionPlans",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<double>(
                name: "CaloriestTarget",
                table: "nutritionPlans",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<int>(
                name: "ServingSize",
                table: "Food",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<double>(
                name: "CaloriesPerServing",
                table: "Food",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Food",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsCeleryFree",
                table: "Food",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDairyFree",
                table: "Food",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFishFree",
                table: "Food",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFruitFree",
                table: "Food",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsGlutenFree",
                table: "Food",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsKosher",
                table: "Food",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLegumeFree",
                table: "Food",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLowFODMAP",
                table: "Food",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLupinFree",
                table: "Food",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMolluscsFree",
                table: "Food",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMustardFree",
                table: "Food",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsNutFree",
                table: "Food",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPeanutFree",
                table: "Food",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSesameFree",
                table: "Food",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsShellfishFree",
                table: "Food",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSoyFree",
                table: "Food",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSulfiteFree",
                table: "Food",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVegan",
                table: "Food",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVegetarian",
                table: "Food",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsWheatFree",
                table: "Food",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "dailyIntakes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalCalories = table.Column<double>(type: "float", nullable: false),
                    TotalProtein = table.Column<double>(type: "float", nullable: false),
                    TotalCarbs = table.Column<double>(type: "float", nullable: false),
                    TotalFat = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dailyIntakes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogFood",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FoodId = table.Column<int>(type: "int", nullable: false),
                    MyProperty = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    LoggedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogFood", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogFood_Food_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Food",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogFood_FoodId",
                table: "LogFood",
                column: "FoodId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dailyIntakes");

            migrationBuilder.DropTable(
                name: "LogFood");

            migrationBuilder.DropColumn(
                name: "GymFrequency",
                table: "userMetrics");

            migrationBuilder.DropColumn(
                name: "CaloriesPerServing",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "IsCeleryFree",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "IsDairyFree",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "IsFishFree",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "IsFruitFree",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "IsGlutenFree",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "IsKosher",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "IsLegumeFree",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "IsLowFODMAP",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "IsLupinFree",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "IsMolluscsFree",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "IsMustardFree",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "IsNutFree",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "IsPeanutFree",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "IsSesameFree",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "IsShellfishFree",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "IsSoyFree",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "IsSulfiteFree",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "IsVegan",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "IsVegetarian",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "IsWheatFree",
                table: "Food");

            migrationBuilder.RenameColumn(
                name: "ProteinPerServing",
                table: "Food",
                newName: "Protein");

            migrationBuilder.RenameColumn(
                name: "FatPerServing",
                table: "Food",
                newName: "Fats");

            migrationBuilder.RenameColumn(
                name: "CarbsPerServing",
                table: "Food",
                newName: "Carbs");

            migrationBuilder.AlterColumn<float>(
                name: "ProteinTarget",
                table: "nutritionPlans",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<float>(
                name: "FatTarget",
                table: "nutritionPlans",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<float>(
                name: "CarbsTarget",
                table: "nutritionPlans",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<float>(
                name: "CaloriestTarget",
                table: "nutritionPlans",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<float>(
                name: "ServingSize",
                table: "Food",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "BarCode",
                table: "Food",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Calories",
                table: "Food",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
