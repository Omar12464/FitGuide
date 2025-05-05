using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class addallergy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AllergyId",
                table: "userAllergies",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_userAllergies_AllergyId",
                table: "userAllergies",
                column: "AllergyId");

            migrationBuilder.AddForeignKey(
                name: "FK_userAllergies_Allergy_AllergyId",
                table: "userAllergies",
                column: "AllergyId",
                principalTable: "Allergy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userAllergies_Allergy_AllergyId",
                table: "userAllergies");

            migrationBuilder.DropIndex(
                name: "IX_userAllergies_AllergyId",
                table: "userAllergies");

            migrationBuilder.AlterColumn<string>(
                name: "AllergyId",
                table: "userAllergies",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
