using Microsoft.AspNetCore.Mvc;
using BTS.Services;
using BTS.Models;
using System.Threading.Tasks;

namespace BTS.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    private readonly ApiClient _apiClient;

    public TestController(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var (success, token) = await _apiClient.Login(model.Username, model.Password);
        if (success)
        {
            return Ok(new { Token = token });
        }
        return Unauthorized(new { Message = "Invalid credentials" });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var success = await _apiClient.Register(model.Email, model.Username, model.Password);
        if (success)
        {
            return Ok(new { Message = "User registered successfully" });
        }
        return BadRequest(new { Message = "Registration failed" });
    }

    [HttpPost("checklist")]
    public async Task<IActionResult> CreateChecklist([FromBody] CreateChecklistModel model)
    {
        try
        {
            var checklist = await _apiClient.CreateChecklist(model.Name);
            return Ok(checklist);
        }
        catch
        {
            return BadRequest(new { Message = "Failed to create checklist" });
        }
    }

    [HttpGet("checklists")]
    public async Task<IActionResult> GetAllChecklists()
    {
        try
        {
            var checklists = await _apiClient.GetAllChecklists();
            return Ok(checklists);
        }
        catch
        {
            return BadRequest(new { Message = "Failed to get checklists" });
        }
    }

    
}
