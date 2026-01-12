using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt_dotnet.Data;
using Projekt_dotnet.Models.DTOs;
using Projekt_dotnet.Services;

namespace Projekt_dotnet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistsController : ControllerBase
    {
        private readonly IPlaylistService _playlistService;
        private readonly ApplicationDbContext _db;

        public PlaylistsController(IPlaylistService playlistService, ApplicationDbContext db)
        {
            _playlistService = playlistService;
            _db = db;
        }

        private string? GetUserId() =>
            User.Claims.LastOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        [HttpGet]
        public async Task<IActionResult> GetPublic()
        {
            var playlists = await _playlistService.GetPublicAsync();
            return Ok(playlists.Select(p => new
            {
                p.Id,
                p.Name,
                p.IsPublic,
                p.CreatedAt,
                Songs = p.PlaylistSongs.Select(ps => new
                {
                    ps.Song.Id,
                    ps.Song.Title,
                    ps.Song.Artist,
                    ps.Song.Year,
                    ps.Song.Genre,
                    ps.Song.FileUrl,
                    ps.Order
                }).ToList()
            }));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePlaylistDto dto)
        {
            var userId = GetUserId();
            var (success, playlist, error) = await _playlistService.CreateAsync(dto, userId!);
            if (!success || playlist == null)
                return BadRequest(new { message = error ?? "Cannot create playlist" });
            return Ok(playlist);
        }

        [Authorize]
        [HttpGet("my")]
        public async Task<IActionResult> GetMy()
        {
            var userId = GetUserId();
            var playlists = await _playlistService.GetMyAsync(userId!);
            return Ok(playlists.Select(p => new
            {
                p.Id,
                p.Name,
                p.IsPublic,
                p.CreatedAt,
                Songs = p.PlaylistSongs.Select(ps => new
                {
                    ps.Song.Id,
                    ps.Song.Title,
                    ps.Song.Artist,
                    ps.Song.Year,
                    ps.Song.Genre,
                    ps.Song.FileUrl,
                    ps.Order
                }).ToList()
            }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var userId = GetUserId();
            var playlist = await _playlistService.GetOneAsync(id, userId!);
            
            // Jeśli nie twoja, sprawdź czy publiczna
            if (playlist == null)
            {
                playlist = await _db.Playlists
                    .Include(p => p.PlaylistSongs.OrderBy(ps => ps.Order))
                    .ThenInclude(ps => ps.Song)
                    .FirstOrDefaultAsync(p => p.Id == id && p.IsPublic);
                
                if (playlist == null)
                    return NotFound();
            }
            
            return Ok(new
            {
                playlist.Id,
                playlist.Name,
                playlist.IsPublic,
                playlist.CreatedAt,
                Songs = playlist.PlaylistSongs
                    .OrderBy(ps => ps.Order)
                    .Select(ps => new
                    {
                        ps.Song.Id,
                        ps.Song.Title,
                        ps.Song.Artist,
                        ps.Song.Year,
                        ps.Song.Genre,
                        ps.Song.FileUrl,
                        ps.Order
                    }).ToList()
            });
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();
            var (success, error) = await _playlistService.DeleteAsync(id, userId!);
            if (!success)
                return BadRequest(new { message = error ?? "Cannot delete playlist" });
            return NoContent();
        }

        [Authorize]
        [HttpPost("{playlistId}/songs")]
        public async Task<IActionResult> AddSong(int playlistId, [FromBody] AddSongToPlaylistDto dto)
        {
            var userId = GetUserId();
            var (success, error) = await _playlistService.AddSongAsync(playlistId, dto.SongId, userId!);
            if (!success)
                return BadRequest(new { message = error ?? "Cannot add song" });
            return Ok(new { message = "Song added" });
        }

        [Authorize]
        [HttpDelete("{playlistId}/songs/{songId}")]
        public async Task<IActionResult> RemoveSong(int playlistId, int songId)
        {
            var userId = GetUserId();
            var (success, error) = await _playlistService.RemoveSongAsync(playlistId, songId, userId!);
            if (!success)
                return BadRequest(new { message = error ?? "Cannot remove song" });
            return NoContent();
        }
    }
}