using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Presistance.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presistance.Repositories
{
    public class StudentRepository(storeDbContext dbContext) : IStudentRepository
    {
        public async Task<Student?> GetByAcademicIdAsync(int academicId)
        {
            return await dbContext.Users.OfType<Student>()
                .Include(s => s.StudentCourses)
                .FirstOrDefaultAsync(s => s.StudentAcademicId == academicId);
        }

        public async Task<Student?> GetByEmailAsync(string email)
        {
            return await dbContext.Users.OfType<Student>()
                .FirstOrDefaultAsync(s => s.Email == email);
        }

        public async Task<Student?> GetStudentWithCoursesAsync(int userId)
        {
            return await dbContext.Users.OfType<Student>()
                .Include(s => s.StudentCourses)
                .FirstOrDefaultAsync(s => s.Id == userId);
        }
    }
}
