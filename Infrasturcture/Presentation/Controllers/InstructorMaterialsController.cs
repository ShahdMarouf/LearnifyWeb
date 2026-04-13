using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstarction;
using Shared.DTOs;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/instructor/materials")]
    [Authorize(Roles = "Instructor")]
    public class InstructorMaterialsController : ControllerBase
    {
        private readonly IMaterialService _materialService;

        public InstructorMaterialsController(IMaterialService materialService)
        {
            _materialService = materialService;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] UploadMaterialDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            if (!int.TryParse(userIdClaim.Value, out int instructorId))
                return Unauthorized();

            var result = await _materialService.UploadMaterialAsync(instructorId, dto);

            return result switch
            {
                MaterialUploadResult.Success => Ok(new { message = "Material uploaded successfully." }),
                MaterialUploadResult.CourseNotFound => NotFound(new { message = $"Course with code {dto.CourseCode} not found." }),
                MaterialUploadResult.NotAuthorized => Forbid(),
                MaterialUploadResult.InvalidFile => BadRequest(new { message = "Invalid file. Ensure it is a PDF and less than 20MB." }),
                _ => BadRequest(new { message = "Failed to upload material." })
            };
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            if (!int.TryParse(userIdClaim.Value, out int instructorId))
                return Unauthorized();

            var stats = await _materialService.GetInstructorDashboardStatsAsync(instructorId);
            return Ok(stats);
        }
    }
}
