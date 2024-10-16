using System;
using Microsoft.AspNetCore.Mvc;
using BTS.Models;
using BTS.Services;
using System.Threading.Tasks;

namespace BTS.Controllers;

[ApiController]
[Route("api")]
public class AuthController : ControllerBase
{
    private readonly ApiClient _apiClient;

    public AuthController(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        try
        {
            var result = await _apiClient.Login(model.Username, model.Password);
            return Ok(new { Token = result.Token });
        }
        catch (HttpRequestException)
        {
            return Unauthorized();
        }
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

    // ... other methods
}
