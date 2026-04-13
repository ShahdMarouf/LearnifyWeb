using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presistance.Migrations
{
    /// <inheritdoc />
    public partial class AddInstructorToCourseOffering : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InstructorId",
                table: "CourseOfferings",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "CourseOfferings");
        }
    }
}
