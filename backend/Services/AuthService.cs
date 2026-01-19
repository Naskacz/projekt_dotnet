using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Projekt_dotnet.Models.DTOs;

namespace Projekt_dotnet.Services
{
    public interface IAuthService
    {
        Task<(bool Success, string? Token, string? Error)> RegisterAsync(RegisterDto dto);
        Task<(bool Success, string? Token, string? Error)> LoginAsync(LoginDto dto);
        Task<(bool Success, string? Token, string? Error)> RefreshAsync(string token);
    }

    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<(bool Success, string? Token, string? Error)> RegisterAsync(RegisterDto dto)
        {
            var userExists = await _userManager.FindByEmailAsync(dto.Email);
            if (userExists != null)
            {
                return (false, null, "User already exists");
            }
            
            var user = new IdentityUser { UserName = dto.Username, Email = dto.Email };
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, null, errors);
            }

            return (true, null, null);
        }

        public async Task<(bool Success, string? Token, string? Error)> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                return (false, null, "Invalid credentials");
            }

            var token = GenerateJwtToken(user);
            return (true, token, null);
        }

        public async Task<(bool Success, string? Token, string? Error)> RefreshAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                    return (false, null, "Invalid token");

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return (false, null, "User not found");

                var newToken = GenerateJwtToken(user);
                return (true, newToken, null);
            }
            catch (SecurityTokenExpiredException)
            {
                return (false, null, "Token expired");
            }
            catch (Exception)
            {
                return (false, null, "Invalid token");
            }
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(_configuration.GetValue<double>("Jwt:ExpireHours")),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
