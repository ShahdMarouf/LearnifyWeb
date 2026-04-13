using System;

namespace Shared.DTOs
{
    public class InstructorMaterialDashboardDto
    {
        public int InstructorId { get; set; }
        public int TotalUploads { get; set; }
        public int TotalLectures { get; set; }
        public int TotalSections { get; set; }
        public DateTime? LatestUploadDate { get; set; }
        public int UploadsThisMonth { get; set; }
        public int DistinctCoursesCount { get; set; }
    }
}
