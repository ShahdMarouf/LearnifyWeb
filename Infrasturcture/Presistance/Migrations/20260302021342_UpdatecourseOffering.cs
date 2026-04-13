using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presistance.Migrations
{
    /// <inheritdoc />
    public partial class UpdatecourseOffering : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CourseOfferings_InstructorId",
                table: "CourseOfferings",
                column: "InstructorId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseOfferings_Users_InstructorId",
                table: "CourseOfferings",
                column: "InstructorId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseOfferings_Users_InstructorId",
                table: "CourseOfferings");

            migrationBuilder.DropIndex(
                name: "IX_CourseOfferings_InstructorId",
                table: "CourseOfferings");
        }
    }
}
