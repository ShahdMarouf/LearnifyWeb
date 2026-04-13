using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presistance.Migrations
{
    /// <inheritdoc />
    public partial class FixCourseMaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InstructorId",
                table: "CourseMaterials",
                newName: "WeekNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WeekNumber",
                table: "CourseMaterials",
                newName: "InstructorId");
        }
    }
}
