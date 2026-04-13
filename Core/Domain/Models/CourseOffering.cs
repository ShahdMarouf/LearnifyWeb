using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class CourseOffering
    {
        public int Id { get; set; }

        public string CourseCode { get; set; }
        public Course Course { get; set; }

        public string Semester { get; set; }       // Fall / Spring / Summer
        public string AcademicYear { get; set; }   // 2024-2025
        public int? InstructorId { get; set; }
        public Instructor Instructor { get; set; }
        public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
        public ICollection<CourseMaterial> Materials { get; set; } = new List<CourseMaterial>();
    }
}
