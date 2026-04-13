using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

[ApiController]
[Route("api/AIchatbot")]
public class AiChatbotController : ControllerBase
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

    // 🔹 Upload (بيرجع sessionId)
    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        var content = new MultipartFormDataContent();
        content.Add(new StreamContent(file.OpenReadStream()), "files", file.FileName);

        var res = await _httpClient.PostAsync("http://localhost:8001/ai/upload", content);
        var data = await res.Content.ReadAsStringAsync();

        return Ok(data);
    }

    // 🔹 Chat
    [HttpPost("chat")]
    public async Task<IActionResult> Chat([FromBody] AiChatRequest request)
    {
        var res = await _httpClient.PostAsJsonAsync(
            "http://localhost:8001/ai/chat",
            request
        );

        var data = await res.Content.ReadAsStringAsync();

        return Ok(data);
    }
}