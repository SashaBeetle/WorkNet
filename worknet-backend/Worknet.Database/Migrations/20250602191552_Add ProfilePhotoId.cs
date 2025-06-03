using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Worknet.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddProfilePhotoId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePhotoId",
                table: "Profiles",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePhotoId",
                table: "Profiles");
        }
    }
}
