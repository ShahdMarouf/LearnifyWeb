namespace Shared.DTOs
{
    public class InstructorLoginResponseDto
    {
        public string Token { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
