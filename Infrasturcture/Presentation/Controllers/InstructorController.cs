using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstarction;
using Shared.DTOs;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorController : ControllerBase
    {
        private readonly IInstructorService _instructorService;

        public InstructorController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] InstructorLoginDto dto)
        {
            try
            {
                var result = await _instructorService.LoginAsync(dto);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                if (ex.Message.Contains("Forbidden"))
                {
                    return StatusCode(403, new { message = ex.Message });
                }
                return Unauthorized(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Instructor")]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateInstructorDto dto)
        {
            try
            {
                var instructorIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(instructorIdClaim) || !int.TryParse(instructorIdClaim, out int instructorId))
                {
                    return Unauthorized(new { message = "Invalid user token" });
                }

                await _instructorService.UpdateProfileAsync(instructorId, dto);
                return Ok(new { message = "Profile updated successfully" });
            }
            catch (System.Exception ex)
            {
                if (ex.Message.Contains("not found"))
                {
                    return NotFound(new { message = ex.Message });
                }
                if (ex.Message.Contains("taken"))
                {
                    return BadRequest(new { message = ex.Message });
                }
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Instructor")]
        [HttpPost("photo")]
        public async Task<IActionResult> UploadPhoto(IFormFile file)
        {
            try
            {
                var instructorIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(instructorIdClaim, out int instructorId))
                    return Unauthorized();

                var path = await _instructorService.UploadPhotoAsync(instructorId, file);
                return Ok(new { message = "Photo uploaded successfully", path });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Instructor")]
        [HttpPut("photo")]
        public async Task<IActionResult> UpdatePhoto(IFormFile file)
        {
            return await UploadPhoto(file); // Replace logic is handled in the service
        }

        [Authorize(Roles = "Instructor")]
        [HttpDelete("photo")]
        public async Task<IActionResult> DeletePhoto()
        {
            try
            {
                var instructorIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(instructorIdClaim, out int instructorId))
                    return Unauthorized();

                await _instructorService.DeletePhotoAsync(instructorId);
                return Ok(new { message = "Photo deleted successfully" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Instructor")]
        [HttpGet("photo")]
        public async Task<IActionResult> GetMyPhoto()
        {
            var instructorIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(instructorIdClaim, out int instructorId))
                return Unauthorized();

            var photoPath = await _instructorService.GetPhotoUrlAsync(instructorId);
            if (string.IsNullOrEmpty(photoPath))
                return NotFound(new { message = "No profile photo found" });

            return Ok(new { url = photoPath });
        }

        [Authorize(Roles = "Instructor")]
        [HttpGet("courses/{courseCode}/students")]
        public async Task<IActionResult> GetStudentsInCourse(string courseCode)
        {
            try
            {
                var students = await _instructorService.GetStudentsByCourseAsync(courseCode);
                return Ok(students);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
