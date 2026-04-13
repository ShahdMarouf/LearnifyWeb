using Domain.Contracts;
using Domain.Models;
using Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using Presistance.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presistance.Repositories
{
    public class StudentCourseRepository(storeDbContext _dbContext) : IStudentCourseRepository
    {
        public async Task<IEnumerable<StudentCourse>> GetMyCoursesAsync(
      int studentId,
      string semester,
      string academicYear)
        {
            return await _dbContext.StudentCourses
                .Where(sc =>
                    sc.StudentId == studentId &&
                    sc.Semester == semester &&
                    sc.AcademicYear == academicYear)
                .ToListAsync();
        }
        public async Task<IEnumerable<InstructorCourseStudentDto>> GetStudentsByCourseAsync(string courseCode)
        {
            return await _dbContext.StudentCourses
                .Where(sc => sc.CourseCode == courseCode)
                .Include(sc => sc.Student)
                .Select(sc => new InstructorCourseStudentDto
                {
                    StudentId = sc.StudentId,
                    FullName = sc.Student.FullName,
                    Email = sc.Student.Email.Replace("@must.edu.eg", ""),
                    Grade = sc.Grade,
                    Passed = sc.Grade != null && sc.Grade != "F"
                })
                .ToListAsync();
        }
    }
}
