using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs
{
    public class UploadMaterialDto
    {
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        [RegularExpression("^(Lecture|Section)$", ErrorMessage = "Type must be either 'Lecture' or 'Section'")]
        public string Type { get; set; } = null!; // Lecture or Section

        [Required]
        public string CourseCode { get; set; } = null!;

        [Required]
        public int WeekNumber { get; set; }

        [Required]
        public IFormFile File { get; set; } = null!;
    }
}
