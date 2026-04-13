using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstarction;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/student/materials")]
    [Authorize(Roles = "Student")]
    public class StudentMaterialsController : ControllerBase
    {
        private readonly IMaterialService _materialService;

        public StudentMaterialsController(IMaterialService materialService)
        {
            _materialService = materialService;
        }

        [HttpGet("{courseCode}")]
        public async Task<IActionResult> GetByCourse(string courseCode)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            if (!int.TryParse(userIdClaim.Value, out int studentId))
                return Unauthorized();

            var result = await _materialService.GetMaterialsByCourseAsync(studentId, courseCode);
            if (result == null)
            {
                return Forbid();
            }

            return Ok(result);
        }

        [HttpGet("download/{courseCode}/{weekNumber}/{type}")]
        public async Task<IActionResult> Download(string courseCode, int weekNumber, string type)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            if (!int.TryParse(userIdClaim.Value, out int studentId))
                return Unauthorized();

            var result = await _materialService.DownloadMaterialAsync(studentId, courseCode, weekNumber, type);
            if (result == null)
            {
                return NotFound(new { message = "Material not found or you are not enrolled in this course." });
            }

            return File(result.Value.FileContents, result.Value.ContentType, result.Value.FileName);
        }
    }
}
