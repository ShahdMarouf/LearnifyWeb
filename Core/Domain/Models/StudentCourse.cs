using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class StudentCourse
    {

        public int Id { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public string CourseCode { get; set; } = null!;

        public virtual ICollection<StudentSchedule> StudentSchedules { get; set; } = new List<StudentSchedule>();

        public string Semester { get; set; } = null!;
        public string AcademicYear { get; set; }

        public string Grade { get; set; }
        public double Points { get; set; }
    }

}
