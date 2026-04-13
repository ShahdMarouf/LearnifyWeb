using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Presistance.Data.Contexts;
using Shared.DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Presistance.Repositories
{
    public class MaterialRepository(storeDbContext _dbContext) : IMaterialRepository
    {
        public async Task<InstructorMaterialDashboardDto> GetInstructorDashboardStatsAsync(int instructorId)
        {
            var now = DateTime.UtcNow;
            var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);

            var stats = await _dbContext.Set<CourseMaterial>()
                .Where(m => m.InstructorId == instructorId)
                .GroupBy(m => m.InstructorId)
                .Select(g => new InstructorMaterialDashboardDto
                {
                    InstructorId = instructorId,
                    TotalUploads = g.Count(),
                    TotalLectures = g.Count(m => m.Type == "Lecture"),
                    TotalSections = g.Count(m => m.Type == "Section"),
                    LatestUploadDate = g.Max(m => (DateTime?)m.CreatedAt),
                    UploadsThisMonth = g.Count(m => m.CreatedAt >= firstDayOfMonth),
                    DistinctCoursesCount = g.Select(m => m.CourseCode).Distinct().Count()
                })
                .FirstOrDefaultAsync();

            return stats ?? new InstructorMaterialDashboardDto
            {
                InstructorId = instructorId,
                TotalUploads = 0,
                TotalLectures = 0,
                TotalSections = 0,
                LatestUploadDate = null,
                UploadsThisMonth = 0,
                DistinctCoursesCount = 0
            };
        }
    }
}
