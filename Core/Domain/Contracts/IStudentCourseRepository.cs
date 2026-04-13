using Domain.Models;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IStudentCourseRepository
    {
        Task<IEnumerable<StudentCourse>> GetMyCoursesAsync(
            int studentId,
            string semester,
            string academicYear);

        Task<IEnumerable<InstructorCourseStudentDto>> GetStudentsByCourseAsync(string courseCode);
    }
}
