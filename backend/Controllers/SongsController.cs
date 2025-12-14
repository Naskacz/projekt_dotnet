using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projekt_dotnet.Models;
using Projekt_dotnet.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

[ApiController]
[Route("api/[controller]")]
public class SongsController : ControllerBase
{
    private readonly SupabaseService _supabaseService;
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;

    public SongsController(SupabaseService supabaseService, ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
    {
        _supabaseService = supabaseService;
        _dbContext = dbContext;
        _userManager = userManager;
    }
    public class UploadSongDto
    {
        public required IFormFile File { get; set; }
        public required string Title { get; set; }
        public required string Artist { get; set; }
        public required int Year { get; set; }
        public required string Genre { get; set; }
    }
    [Authorize]
    [HttpPost("upload")]
    [RequestSizeLimit(50_000_000)]
    public async Task<IActionResult> UploadSong([FromForm] UploadSongDto dto)
    {
        if (dto.File == null || dto.File.Length == 0)
            return BadRequest("No file uploaded.");

        string? uploadedUrl = null;
        Song? song = null;

        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            string userId = User.Identity?.Name ?? throw new Exception("User not found");
            using var stream = dto.File.OpenReadStream();
            uploadedUrl = await _supabaseService.UploadSongAsync(stream, dto.File.FileName);
            if (string.IsNullOrWhiteSpace(uploadedUrl))
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "File upload failed.");
            }
            song = new Song
            {
                Title = dto.Title,
                Artist = dto.Artist,
                Year = dto.Year,
                Genre = dto.Genre,
                FileName = dto.File.FileName,
                FileUrl = uploadedUrl,
                CreatedById = string.IsNullOrEmpty(userId) ? null : userId
            };
            _dbContext.Songs.Add(song);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return Ok(new { uploadedUrl, id = song.Id });
        }
        catch(Exception)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, "An error occurred while uploading the song.");
        }
    }
    [HttpGet]
    public async Task<IActionResult> GetAllSongs()
    {
        var songs = await _dbContext.Songs.ToListAsync();
        return Ok(songs);
    }
}


