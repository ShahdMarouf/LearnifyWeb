using System.Collections.Generic;

namespace Shared.DTOs
{
    public class StudentMaterialResponseDto
    {
        public string CourseCode { get; set; } = null!;
        public List<WeekMaterialDto> Weeks { get; set; } = new();
    }

    public class WeekMaterialDto
    {
        public int WeekNumber { get; set; }
        public int? LectureMaterialId { get; set; }
        public int? SectionMaterialId { get; set; }
    }
}
