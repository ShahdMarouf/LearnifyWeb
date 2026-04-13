using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstarction;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/materials")]
    [Authorize]
    public class MaterialsController : ControllerBase
    {
        private readonly IMaterialService _materialService;

        public MaterialsController(IMaterialService materialService)
        {
            _materialService = materialService;
        }

        [HttpGet("download/{courseCode}/{weekNumber}/{type}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Download(string courseCode, int weekNumber, string type)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            if (!int.TryParse(userIdClaim.Value, out int studentId))
                return Unauthorized();

            var result = await _materialService.DownloadMaterialAsync(studentId, courseCode, weekNumber, type);
            if (result == null)
            {
                return NotFound(new { message = "Material not found or access denied." });
            }

            return File(result.Value.FileContents, result.Value.ContentType, result.Value.FileName);
        }
    }
}
