namespace Shared.DTOs
{
    public class InstructorCourseStudentDto
    {
        public int StudentId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Grade { get; set; }
        public bool Passed { get; set; }
    }
}
