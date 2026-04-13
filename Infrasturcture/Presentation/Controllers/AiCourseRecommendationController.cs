using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/AIcourseRecommendation")]
public class AiCourseRecommendationController : ControllerBase
{
    private readonly HttpClient _httpClient = new HttpClient();

    // 🔹 Health
    [HttpGet("health")]
    public async Task<IActionResult> Health()
    {
        var res = await _httpClient.GetAsync("http://localhost:8003/health");
        var data = await res.Content.ReadAsStringAsync();
        return Ok(data);
    }

    // 🔹 Get Students
    [HttpGet("students")]
    public async Task<IActionResult> Students()
    {
        var res = await _httpClient.GetAsync("http://localhost:8003/students");
        var data = await res.Content.ReadAsStringAsync();
        return Ok(data);
    }

    // 🔹 Upload Course Files
    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        var content = new MultipartFormDataContent();
        content.Add(new StreamContent(file.OpenReadStream()), "files", file.FileName);

        var res = await _httpClient.PostAsync("http://localhost:8003/ai/course/upload", content);
        var data = await res.Content.ReadAsStringAsync();

        return Ok(data);
    }

    // 🔹 Recommend Courses
    [HttpPost("recommend")]
    [Authorize]
    public async Task<IActionResult> Recommend()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            return Unauthorized();

        var request = new
        {
            userId = userId
        };

        var res = await _httpClient.PostAsJsonAsync(
            "http://localhost:8003/ai/course/recommendations",
            request
        );

        var data = await res.Content.ReadAsStringAsync();

        return Ok(data);
    }
}
