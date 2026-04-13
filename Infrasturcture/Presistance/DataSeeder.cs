using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Presistance.Data.Contexts;
using ServiceAbstarction;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Presistance
{
    public class DataSeeder(storeDbContext context, IPasswordService passwordService) : IDataSeed
    {
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };
        private const string DataPath = @"..\Infrasturcture\Presistance\Data\DataSeeding";

        public async Task SeedStudentsAsync()
        {
            await MigrateDatabaseAsync();
            await SeedCoursesAsync();
            await SeedCourseOfferingsAsync();
            await SeedStudentsOnlyAsync();
            await SeedStudentCoursesAsync();
            await SeedStudentSchedulesAsync();
        }

        public async Task SeedStudentCoursesAsync()
        {
            Console.WriteLine("Seeding StudentCourses...");

            if (await context.StudentCourses.AnyAsync())
            {
                Console.WriteLine("StudentCourses already seeded. Skipping.");
                return;
            }

            var filePath = Path.Combine(DataPath, "StudentCourses.json");
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return;
            }

            var json = await File.ReadAllTextAsync(filePath);
            var studentCourseDtos = JsonSerializer.Deserialize<List<StudentCourseDTO>>(json, _jsonOptions) ?? [];

            const int batchSize = 500;
            for (int i = 0; i < studentCourseDtos.Count; i += batchSize)
            {
                var batch = studentCourseDtos.Skip(i).Take(batchSize).ToList();
                List<StudentCourse> studentCoursesToSeed = [];

                foreach (var dto in batch)
                {
                    if (!int.TryParse(dto.StudentAcademicId, out int academicId)) continue;

                    var studentId = await context.Users
                        .OfType<Student>()
                        .Where(s => s.StudentAcademicId == academicId)
                        .Select(s => s.Id)
                        .FirstOrDefaultAsync();

                    if (studentId == 0) continue;

                    studentCoursesToSeed.Add(new StudentCourse
                    {
                        StudentId = studentId,
                        CourseCode = dto.CourseCode,
                        Grade = dto.Grade,
                        Semester = dto.Semester,
                        AcademicYear = dto.AcademicYear
                    });
                }

                if (studentCoursesToSeed.Count > 0)
                {
                    context.StudentCourses.AddRange(studentCoursesToSeed);
                    await context.SaveChangesAsync();
                }
            }
            Console.WriteLine("StudentCourses seeded ✅");
        }

        public async Task SeedStudentSchedulesAsync()
        {
            Console.WriteLine("Seeding StudentSchedules...");

            if (await context.StudentSchedules.AnyAsync())
            {
                Console.WriteLine("StudentSchedules already seeded. Skipping.");
                return;
            }

            var filePath = Path.Combine(DataPath, "StudentSchedules.json");
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return;
            }

            var json = await File.ReadAllTextAsync(filePath);
            var scheduleDtos = JsonSerializer.Deserialize<List<StudentScheduleSeedDto>>(json, _jsonOptions) ?? [];

            List<StudentSchedule> schedulesToSeed = [];

            foreach (var dto in scheduleDtos)
            {
                if (!int.TryParse(dto.StudentAcademicId, out int academicId)) continue;

                var studentCourse = await context.StudentCourses
                    .FirstOrDefaultAsync(sc => sc.Student.StudentAcademicId == academicId &&
                                             sc.CourseCode == dto.CourseCode &&
                                             sc.Semester == dto.Semester &&
                                             sc.AcademicYear == dto.AcademicYear);

                if (studentCourse == null) continue;

                schedulesToSeed.Add(new StudentSchedule
                {
                    StudentCourseId = studentCourse.Id,
                    Day = dto.Day,
                    StartTime = TimeSpan.Parse(dto.StartTime),
                    EndTime = TimeSpan.Parse(dto.EndTime),
                    Type = dto.Type,
                    Hall = dto.Hall
                });
            }

            if (schedulesToSeed.Count > 0)
            {
                context.StudentSchedules.AddRange(schedulesToSeed);
                await context.SaveChangesAsync();
                Console.WriteLine("StudentSchedules seeded ✅");
            }
        }

        private async Task MigrateDatabaseAsync()
        {
            if ((await context.Database.GetPendingMigrationsAsync()).Any())
            {
                Console.WriteLine("Applying pending migrations...");
                await context.Database.MigrateAsync();
            }
        }

        private async Task SeedCoursesAsync()
        {
            if (await context.Courses.AnyAsync()) return;

            var filePath = Path.Combine(DataPath, "Courses.json");
            var json = await File.ReadAllTextAsync(filePath);
            var courses = JsonSerializer.Deserialize<List<CourseDTo>>(json, _jsonOptions) ?? [];

            var coursesToSeed = courses.Select(c => new Course
            {
                CourseCode = c.CourseCode,
                FullName = c.CourseName,
                Credits = c.Credits
            }).ToList();

            context.Courses.AddRange(coursesToSeed);
            await context.SaveChangesAsync();
        }

        private async Task SeedCourseOfferingsAsync()
        {
            if (await context.CourseOffering.AnyAsync()) return;

            var filePath = Path.Combine(DataPath, "CourseOfferings.json");
            var json = await File.ReadAllTextAsync(filePath);
            var offerings = JsonSerializer.Deserialize<List<CourseOfferingDTO>>(json, _jsonOptions) ?? [];

            var offeringsToSeed = offerings.Select(o => new CourseOffering
            {
                CourseCode = o.CourseCode,
                Semester = o.Semester,
                AcademicYear = o.AcademicYear
            }).ToList();

            context.CourseOffering.AddRange(offeringsToSeed);
            await context.SaveChangesAsync();
        }

        private async Task SeedStudentsOnlyAsync()
        {
            if (await context.Users.AnyAsync(u => u.Role == "Student")) return;

            var filePath = Path.Combine(DataPath, "Students.json");
            var json = await File.ReadAllTextAsync(filePath);
            var studentsFromFile = JsonSerializer.Deserialize<List<StudentSeedDto>>(json, _jsonOptions) ?? [];

            var instructor = await context.Users
                .OfType<Instructor>()
                .FirstOrDefaultAsync(u => u.Email == "instructor@system.com");

            if (instructor == null)
            {
                passwordService.CreatePasswordHash("instructor123", out string instHash, out string instSalt);
                instructor = new Instructor
                {
                    FullName = "Default Instructor",
                    Email = "instructor@system.com",
                    PasswordHash = instHash,
                    PasswordSalt = instSalt,
                    Role = "Instructor"
                };
                context.Users.Add(instructor);
                await context.SaveChangesAsync();
            }

            List<Student> students = [];
            foreach (var s in studentsFromFile)
            {
                var academicIdString = s.Email.Split('@')[0];
                if (!int.TryParse(academicIdString, out int academicId)) continue;

                passwordService.CreatePasswordHash(academicIdString, out string hash, out string salt);

                students.Add(new Student
                {
                    FullName = s.FullName,
                    Email = s.Email,
                    StudentAcademicId = academicId,
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    Role = "Student",
                    InstructorId = instructor.Id,
                    GPA = s.GPA,
                    StudyHours = s.StudyHours,
                    Percentage = s.Percentage
                });
            }

            context.Users.AddRange(students);
            await context.SaveChangesAsync();
        }

        private class StudentScheduleSeedDto
        {
            public string StudentAcademicId { get; set; } = null!;
            public string CourseCode { get; set; } = null!;
            public string Semester { get; set; } = null!;
            public string AcademicYear { get; set; } = null!;
            public string Day { get; set; } = null!;
            public string StartTime { get; set; } = null!;
            public string EndTime { get; set; } = null!;
            public string Type { get; set; } = null!;
            public string Hall { get; set; } = null!;
        }
    }
}
