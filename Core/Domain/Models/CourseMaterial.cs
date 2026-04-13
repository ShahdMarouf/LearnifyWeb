using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class CourseMaterial
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int WeekNumber { get; set; } // IMPORTANT - must be integer
        public string Type { get; set; } = null!; // "Lecture" or "Section"
        public string FilePath { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string CourseCode { get; set; } = null!;
        public int InstructorId { get; set; }
    }
}
