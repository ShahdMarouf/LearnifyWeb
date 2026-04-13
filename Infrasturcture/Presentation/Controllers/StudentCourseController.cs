using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstarction;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Student")]
    public class StudentCourseController : ControllerBase
    {
        private readonly IStudentCourseService _service;

        public StudentCourseController(IStudentCourseService service)
        {
            _service = service;
        }

        [HttpGet("me/courses")]
        public async Task<IActionResult> GetCourses(
    [FromQuery] string semester,
    [FromQuery] string academicYear)
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var courses = await _service
                .GetMyCourses(userId, semester, academicYear);

            return Ok(courses);
        }
    }
}
