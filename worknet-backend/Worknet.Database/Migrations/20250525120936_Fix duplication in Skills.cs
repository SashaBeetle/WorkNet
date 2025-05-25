using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Worknet.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixduplicationinSkills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Profiles_ProfileId1",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Skills_ProfileId1",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "ProfileId1",
                table: "Skills");

            migrationBuilder.AlterColumn<string>(
                name: "ProfileId",
                table: "Skills",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_ProfileId",
                table: "Skills",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Profiles_ProfileId",
                table: "Skills",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Profiles_ProfileId",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Skills_ProfileId",
                table: "Skills");

            migrationBuilder.AlterColumn<int>(
                name: "ProfileId",
                table: "Skills",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "ProfileId1",
                table: "Skills",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_ProfileId1",
                table: "Skills",
                column: "ProfileId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Profiles_ProfileId1",
                table: "Skills",
                column: "ProfileId1",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
