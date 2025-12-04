using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projekt_dotnet.Models;
using Projekt_dotnet.Data;

[ApiController]
[Route("api/[controller]")]
public class SongsController : ControllerBase
{
    private readonly SupabaseService _supabaseService;
    private readonly ApplicationDbContext _dbContext;

    public SongsController(SupabaseService supabaseService, ApplicationDbContext dbContext)
    {
        _supabaseService = supabaseService;
        _dbContext = dbContext;
    }
    public class UploadSongDto
    {
        public IFormFile File { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
    }


    [HttpPost("upload")]
    [RequestSizeLimit(50_000_000)]
    public async Task<IActionResult> UploadSong([FromForm] UploadSongDto dto)
{
    if (dto.File == null || dto.File.Length == 0)
        return BadRequest("No file uploaded.");

    using var stream = dto.File.OpenReadStream();
    var url = await _supabaseService.UploadSongAsync(stream, dto.File.FileName);

    var song = new Song
    {
        Title = dto.Title,
        Artist = dto.Artist,
        Year = dto.Year,
        Genre = dto.Genre,
        FileName = dto.File.FileName,
        FileUrl = url,
    };
        _dbContext.Songs.Add(song);
        await _dbContext.SaveChangesAsync();

        // 3️⃣ Zwrócenie URL do frontendu
        return Ok(new { url, id = song.Id });
    }
}


