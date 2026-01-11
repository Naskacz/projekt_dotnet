using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projekt_dotnet.Models.DTOs;
using Projekt_dotnet.Services;

namespace Projekt_dotnet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongsController : ControllerBase
    {
        private readonly ISongService _songService;
        private readonly SupabaseService _supabaseService;

        public SongsController(ISongService songService, SupabaseService supabaseService)
        {
            _songService = songService;
            _supabaseService = supabaseService;
        }

        private string? GetUserId() => 
            User.Claims.LastOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        [Authorize]
        [HttpPost("upload")]
        [RequestSizeLimit(50_000_000)]
        public async Task<IActionResult> UploadSong([FromForm] CreateSongDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                return BadRequest("No file uploaded.");

            var userId = GetUserId();
            var (success, song, error) = await _songService.UploadSongAsync(dto, userId, _supabaseService);

            if (!success)
                return BadRequest(new { error });

            return Ok(new { uploadedUrl = song.FileUrl, id = song.Id });
        }
        [Authorize]
        [HttpGet("my")]
        public async Task<IActionResult> GetMySongs()
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized();

            var songs = await _songService.GetSongsByUserAsync(userId);
            return Ok(songs);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSongs()
        {
            var songs = await _songService.GetAllSongsAsync();
            return Ok(songs);
        }
        [Authorize]
        [HttpPost("{songId}/add-to-album")]
        public async Task<IActionResult> AddSongToAlbum(int songId, [FromBody] AddToAlbumDto dto)
        {
            var userId = GetUserId();
            var (success, error) = await _songService.AddSongToAlbumAsync(songId, dto.AlbumId, userId);
    
            if (!success)
                return BadRequest(new { error });
    
            return Ok(new { message = "Utwór dodany do albumu" });
        }
    }
}


