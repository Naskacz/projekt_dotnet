using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projekt_dotnet.Models;
using Projekt_dotnet.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]

public class AlbumsController : ControllerBase {
    private readonly SupabaseService _supabaseService;

    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;

    public AlbumsController(SupabaseService supabaseService, ApplicationDbContext dbContext, UserManager<IdentityUser> userManager) {
        _dbContext = dbContext;
        _userManager = userManager;
        _supabaseService = supabaseService;
    }
    public class CreateAlbumDto {
        public required string Name { get; set; }
        public required string Artist { get; set; }
        public required int ReleaseYear { get; set; }
        public string? CoverUrl { get; set; }
    }
    public class AddSongToAlbumDto {
        public required int SongId { get; set; }
    }
    private string? GetUserId() => User.Claims.LastOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreateAlbum([FromForm] CreateAlbumDto dto){
        var userId = GetUserId();
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User not found.");

        var userExists = await _dbContext.Users.AnyAsync(u => u.Id == userId);
        if (!userExists)
            return BadRequest("User ID from token does not exist in database");

        var album = new Album {
            Name = dto.Name,
            Artist = dto.Artist,
            ReleaseYear = dto.ReleaseYear,
            CoverUrl = dto.CoverUrl,
            CreatedById = userId
        };

        _dbContext.Albums.Add(album);
        await _dbContext.SaveChangesAsync();

        return Ok(album);
    }
    [HttpPost("albums")]
    public async Task<IActionResult> GetAlbums() {
        var albums = await _dbContext.Albums
            .Include(a => a.Songs)
            .ToListAsync();
        return Ok(albums);
    }
}