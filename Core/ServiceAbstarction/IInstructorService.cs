using Microsoft.AspNetCore.Http;
using Shared.DTOs;
using System.Threading.Tasks;

namespace ServiceAbstarction
{
    public interface IInstructorService
    {
        Task<InstructorLoginResponseDto?> LoginAsync(InstructorLoginDto dto);
        Task UpdateProfileAsync(int instructorId, UpdateInstructorDto dto);
        Task<string> UploadPhotoAsync(int instructorId, IFormFile file);
        Task DeletePhotoAsync(int instructorId);
        Task<string?> GetPhotoUrlAsync(int instructorId);
        Task<IEnumerable<InstructorCourseStudentDto>> GetStudentsByCourseAsync(string courseCode);
    }
}
