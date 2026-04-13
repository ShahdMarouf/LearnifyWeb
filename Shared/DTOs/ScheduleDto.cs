using System;

namespace Shared.DTOs
{
    public class ScheduleDto
    {
        public string CourseCode { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public string Day { get; set; } = null!;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Type { get; set; } = null!; // Lecture or Section
        public string Hall { get; set; } = null!;
    }
}
