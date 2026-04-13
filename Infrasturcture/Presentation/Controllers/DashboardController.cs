using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstarction;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Infrastructure.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Student")]
    public class DashboardController(IStudentService studentService, IScheduleService scheduleService) : ControllerBase
    {
        [HttpGet("me")]
        public async Task<IActionResult> GetDashboard()
        {
            var academicId = User.FindFirst("StudentAcademicId")?.Value;

            if (string.IsNullOrEmpty(academicId))
            {
                return StatusCode(401, "Student Academic ID not found in token.");
            }

            var dashboard = await studentService.GetStudentDashboardAsync(academicId);

            if (dashboard == null)
            {
                return NotFound("Student dashboard data not found.");
            }

            return Ok(dashboard);
        }

        [HttpGet("me/summary")]
        public async Task<IActionResult> GetMySummary()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var summary = await studentService.GetStudentSummaryAsync(userId);

            if (summary == null)
            {
                return NotFound("Student summary data not found.");
            }

            return Ok(summary);
        }

        [HttpGet("me/schedule")]
        public async Task<IActionResult> GetMySchedule()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var schedule = await scheduleService.GetMyScheduleAsync(userId);

            return Ok(schedule);
        }
    }
}
