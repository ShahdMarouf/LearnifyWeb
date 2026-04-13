using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Student : User
    {
        public int StudentAcademicId { get; set; }
        public int? InstructorId { get; set; }
        public Instructor? Instructor { get; set; }
        public decimal GPA { get; set; }
        public int StudyHours { get; set; }
        public decimal Percentage { get; set; }

        public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
        public ICollection<Note> Notes { get; set; } = new List<Note>();
    }
}
