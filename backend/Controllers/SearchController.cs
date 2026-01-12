using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt_dotnet.Data;
using Projekt_dotnet.Models.DTOs;

namespace Projekt_dotnet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public SearchController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest(new { error = "Query is required" });

            var query = q.ToLower();

            var songs = await _db.Songs
                .Where(s => s.Title.ToLower().Contains(query) || s.Artist.ToLower().Contains(query))
                .Select(s => new { type = "song", s.Id, s.Title, s.Artist, s.Genre })
                .Take(5)
                .ToListAsync();

            var albums = await _db.Albums
                .Where(a => a.Name.ToLower().Contains(query) || a.Artist.ToLower().Contains(query))
                .Select(a => new { type = "album", a.Id, a.Name, a.Artist })
                .Take(5)
                .ToListAsync();

            var playlists = await _db.Playlists
                .Where(p => p.IsPublic && p.Name.ToLower().Contains(query))
                .Select(p => new { type = "playlist", p.Id, p.Name })
                .Take(5)
                .ToListAsync();

            var results = songs.Cast<dynamic>().Concat(albums).Concat(playlists).ToList();

            return Ok(new { results });
        }
    }
}