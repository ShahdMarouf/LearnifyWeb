using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs
{
    public class UpdateInstructorDto
    {
        [Required]
        public string FullName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        public string? NewPassword { get; set; }
    }
}
