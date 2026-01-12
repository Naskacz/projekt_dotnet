using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Projekt_dotnet.Models.DTOs;
using Projekt_dotnet.Services;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var (success, token, error) = await _authService.RegisterAsync(dto);

        if (!success)
            return BadRequest(new { message = error });

        return Ok(new { message = "User registered successfully" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var (success, token, error) = await _authService.LoginAsync(dto);

        if (!success)
            return Unauthorized(new { message = error });

        return Ok(new { token, email = dto.Email });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var authHeader = Request.Headers["Authorization"].FirstOrDefault();
        var token = authHeader?.StartsWith("Bearer ") == true
            ? authHeader["Bearer ".Length..]
            : null;

        if (string.IsNullOrWhiteSpace(token))
            return Unauthorized(new { message = "Missing token" });

        var (success, newToken, error) = await _authService.RefreshAsync(token);

        if (!success || newToken == null)
            return Unauthorized(new { message = error ?? "Invalid token" });

        return Ok(new { token = newToken });
    }
}