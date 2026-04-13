using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presistance.Migrations
{
    /// <inheritdoc />
    public partial class FinalSync : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseMaterials_CourseOfferings_CourseOfferingId",
                table: "CourseMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourses_CourseOfferings_CourseOfferingId",
                table: "StudentCourses");

            migrationBuilder.DropIndex(
                name: "IX_StudentCourses_StudentId_CourseOfferingId",
                table: "StudentCourses");

            migrationBuilder.AlterColumn<string>(
                name: "Semester",
                table: "StudentCourses",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "CourseOfferingId",
                table: "StudentCourses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "AcademicYear",
                table: "StudentCourses",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "CourseOfferingId",
                table: "CourseMaterials",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "CourseCode",
                table: "CourseMaterials",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "InstructorId",
                table: "CourseMaterials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourses_StudentId_CourseCode_Semester_AcademicYear",
                table: "StudentCourses",
                columns: new[] { "StudentId", "CourseCode", "Semester", "AcademicYear" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseMaterials_CourseOfferings_CourseOfferingId",
                table: "CourseMaterials",
                column: "CourseOfferingId",
                principalTable: "CourseOfferings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourses_CourseOfferings_CourseOfferingId",
                table: "StudentCourses",
                column: "CourseOfferingId",
                principalTable: "CourseOfferings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseMaterials_CourseOfferings_CourseOfferingId",
                table: "CourseMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourses_CourseOfferings_CourseOfferingId",
                table: "StudentCourses");

            migrationBuilder.DropIndex(
                name: "IX_StudentCourses_StudentId_CourseCode_Semester_AcademicYear",
                table: "StudentCourses");

            migrationBuilder.DropColumn(
                name: "CourseCode",
                table: "StudentCourses");

            migrationBuilder.DropColumn(
                name: "CourseCode",
                table: "CourseMaterials");

            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "CourseMaterials");

            migrationBuilder.AlterColumn<string>(
                name: "Semester",
                table: "StudentCourses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "CourseOfferingId",
                table: "StudentCourses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AcademicYear",
                table: "StudentCourses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "CourseOfferingId",
                table: "CourseMaterials",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourses_StudentId_CourseOfferingId",
                table: "StudentCourses",
                columns: new[] { "StudentId", "CourseOfferingId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseMaterials_CourseOfferings_CourseOfferingId",
                table: "CourseMaterials",
                column: "CourseOfferingId",
                principalTable: "CourseOfferings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourses_CourseOfferings_CourseOfferingId",
                table: "StudentCourses",
                column: "CourseOfferingId",
                principalTable: "CourseOfferings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
