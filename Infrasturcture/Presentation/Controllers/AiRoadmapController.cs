using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

[ApiController]
[Route("api/AIroadmap")]
public class AiRoadmapController : ControllerBase
{
    private readonly HttpClient _httpClient = new HttpClient();

    // 🔹 Health
    [HttpGet("health")]
    public async Task<IActionResult> Health()
    {
        var res = await _httpClient.GetAsync("http://localhost:8002/health");
        var data = await res.Content.ReadAsStringAsync();
        return Ok(data);
    }

    // 🔹 Upload
    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        var content = new MultipartFormDataContent();
        content.Add(new StreamContent(file.OpenReadStream()), "file", file.FileName);

        var res = await _httpClient.PostAsync("http://localhost:8002/roadmap/upload", content);
        var data = await res.Content.ReadAsStringAsync();

        return Ok(data);
    }

    // 🔹 Careers
    [HttpGet("careers")]
    public async Task<IActionResult> Careers()
    {
        var res = await _httpClient.GetAsync("http://localhost:8002/roadmap/careers");
        var data = await res.Content.ReadAsStringAsync();
        return Ok(data);
    }

    // 🔹 Recommend
    [HttpPost("recommend")]
    public async Task<IActionResult> Recommend([FromBody] object request)
    {
        var res = await _httpClient.PostAsJsonAsync(
            "http://localhost:8002/roadmap/recommend",
            request
        );

        var data = await res.Content.ReadAsStringAsync();
        return Ok(data);
    }

    // 🔹 Plan
    [HttpPost("plan")]
    public async Task<IActionResult> Plan([FromBody] object request)
    {
        var res = await _httpClient.PostAsJsonAsync(
            "http://localhost:8002/roadmap/plan",
            request
        );

        var data = await res.Content.ReadAsStringAsync();
        return Ok(data);
    }
}