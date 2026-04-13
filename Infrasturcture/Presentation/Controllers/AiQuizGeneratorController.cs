using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

[ApiController]
[Route("api/AIquizGenerator")]
public class AIQuizGeneratorController : ControllerBase
{
    private readonly HttpClient _httpClient = new HttpClient();

    // 🔹 Health
    [HttpGet("health")]
    public async Task<IActionResult> Health()
    {
        var res = await _httpClient.GetAsync("http://localhost:8001/health");
        var data = await res.Content.ReadAsStringAsync();
        return Ok(data);
    }

    // 🔹 Upload
    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        var content = new MultipartFormDataContent();
        content.Add(new StreamContent(file.OpenReadStream()), "files", file.FileName);

        var res = await _httpClient.PostAsync("http://localhost:8001/quiz/upload", content);
        var data = await res.Content.ReadAsStringAsync();

        return Ok(data);
    }

    // 🔹 Generate Quiz
    [HttpPost("generate")]
    public async Task<IActionResult> Generate([FromBody] QuizGenrateRequest request)
    {
        var res = await _httpClient.PostAsJsonAsync(
            "http://localhost:8001/quiz/generate",
            request
        );

        var data = await res.Content.ReadAsStringAsync();
        return Ok(data);
    }

    // 🔹 Submit Answers
    [HttpPost("submit")]
    public async Task<IActionResult> Submit([FromBody] QuizSubmitRequest request)
    {
        var res = await _httpClient.PostAsJsonAsync(
            "http://localhost:8001/quiz/submit",
            request
        );

        var data = await res.Content.ReadAsStringAsync();
        return Ok(data);
    }

    // 🔹 Explain
    [HttpPost("explain")]
    public async Task<IActionResult> Explain([FromBody] QuizExplainRequest request)
    {
        var res = await _httpClient.PostAsJsonAsync(
            "http://localhost:8001/quiz/explain",
            request
        );

        var data = await res.Content.ReadAsStringAsync();
        return Ok(data);
    }
}