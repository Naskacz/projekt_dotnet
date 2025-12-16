using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projekt_dotnet.Models.DTOs;
using Projekt_dotnet.Services;

namespace Projekt_dotnet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlbumsController : ControllerBase
    {
        private readonly IAlbumService _albumService;

        public AlbumsController(IAlbumService albumService)
        {
            _albumService = albumService;
        }

        private string? GetUserId() =>
            User.Claims.LastOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateAlbum([FromForm] CreateAlbumDto dto)
        {
            var userId = GetUserId();
            var (success, album, error) = await _albumService.CreateAlbumAsync(dto, userId);

            if (!success)
                return BadRequest(new { error });

            return Ok(new { id = album.Id, album.Name, album.ReleaseYear });
        }

        [HttpGet] 
        public async Task<IActionResult> GetAlbums()
        {
            var albums = await _albumService.GetAllAlbumsAsync();
            return Ok(albums);
        }

        [HttpGet("{albumId:int}")] 
        public async Task<IActionResult> GetAlbum(int albumId)
        {
            var album = await _albumService.GetAlbumByIdAsync(albumId);
            if (album == null) return NotFound();
            return Ok(album);
        }

        [Authorize]
        [HttpDelete("{albumId}")]
        public async Task<IActionResult> DeleteAlbum(int albumId)
        {
            var userId = GetUserId();
            var (success, error) = await _albumService.DeleteAlbumAsync(albumId, userId);

            if (!success)
                return BadRequest(new { error });

            return NoContent();
        }
    }
}