using Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceAbstarction
{
    public enum MaterialUploadResult
    {
        Success,
        CourseNotFound,
        NotAuthorized,
        InvalidFile,
        Failure
    }

    public interface IMaterialService
    {
        Task<MaterialUploadResult> UploadMaterialAsync(int instructorId, UploadMaterialDto dto);
        Task<StudentMaterialResponseDto?> GetMaterialsByCourseAsync(int studentId, string courseCode);
        Task<(byte[] FileContents, string ContentType, string FileName)?> DownloadMaterialAsync(int studentId, string courseCode, int weekNumber, string type);
        Task<InstructorMaterialDashboardDto> GetInstructorDashboardStatsAsync(int instructorId);
    }
}
