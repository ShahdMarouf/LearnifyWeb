using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class StudentCourseDTO
    {
        public string StudentAcademicId { get; set; } = null!;
        public string CourseCode { get; set; } = null!;
        public string Semester { get; set; } = null!;
        public string AcademicYear { get; set; } = null!;
        public string Grade { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public int Credits { get; set; }
    }
}
