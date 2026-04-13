using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstarction
{
    public interface IStudentCourseService
    {
        Task<IEnumerable<StudentCourseDTO>> GetMyCourses(
     int userId,
     string semester,
     string academicYear);
    }
}
