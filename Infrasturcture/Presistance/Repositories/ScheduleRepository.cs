using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Presistance.Data.Contexts;
using Shared.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presistance.Repositories
{
    public class ScheduleRepository(storeDbContext dbContext) : IScheduleRepository
    {
        public async Task<List<ScheduleDto>> GetStudentScheduleAsync(int studentId)
        {
            // Find latest academic year and semester for this student
            var latestTerms = await dbContext.StudentCourses
                .Where(sc => sc.StudentId == studentId)
                .OrderByDescending(sc => sc.AcademicYear)
                .ThenByDescending(sc => sc.Semester) // Note: This simplistic string sort works if Fall/Spring naming is standard or if we strictly use OrderBy.
                .Select(sc => new { sc.AcademicYear, sc.Semester })
                .FirstOrDefaultAsync();

            if (latestTerms == null)
            {
                return new List<ScheduleDto>();
            }

            return await dbContext.StudentSchedules
         .Where(ss => ss.StudentCourse.StudentId == studentId)
         .Select(ss => new ScheduleDto
         {
             CourseCode = ss.StudentCourse.CourseCode,
             CourseName = ss.StudentCourse.CourseCode, // Simplified: use code as name for now or join with Course table
             Day = ss.Day,
             StartTime = ss.StartTime,
             EndTime = ss.EndTime,
             Type = ss.Type,
             Hall = ss.Hall
         })
         .ToListAsync();
        }
    }
}
