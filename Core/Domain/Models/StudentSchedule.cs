using System;

namespace Domain.Models
{
    public class StudentSchedule
    {
        public int Id { get; set; }

        public int StudentCourseId { get; set; }
        public StudentCourse StudentCourse { get; set; } = null!;

        public string Day { get; set; } = null!;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Type { get; set; } = null!; // Lecture / Section
        public string Hall { get; set; } = null!;
    }
}
