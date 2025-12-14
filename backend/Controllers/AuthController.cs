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
}