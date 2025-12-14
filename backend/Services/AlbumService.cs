using Microsoft.EntityFrameworkCore;
using Projekt_dotnet.Data;
using Projekt_dotnet.Models;
using Projekt_dotnet.Models.DTOs;

namespace Projekt_dotnet.Services
{
    public interface IAlbumService
    {
        Task<(bool Success, Album? Album, string? Error)> CreateAlbumAsync(CreateAlbumDto dto, string userId);
        Task<List<dynamic>> GetAllAlbumsAsync();
        Task<(bool Success, string? Error)> DeleteAlbumAsync(int albumId, string userId);
    }

    public class AlbumService : IAlbumService
    {
        private readonly ApplicationDbContext _dbContext;

        public AlbumService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(bool Success, Album? Album, string? Error)> CreateAlbumAsync(CreateAlbumDto dto, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return (false, null, "User not found");

            var userExists = await _dbContext.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
                return (false, null, "User does not exist");

            var album = new Album
            {
                Name = dto.Name,
                Artist = dto.Artist,
                ReleaseYear = dto.ReleaseYear,
                CoverUrl = dto.CoverUrl,
                CreatedById = userId
            };

            try
            {
                _dbContext.Albums.Add(album);
                await _dbContext.SaveChangesAsync();
                return (true, album, null);
            }
            catch (Exception ex)
            {
                return (false, null, ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<List<dynamic>> GetAllAlbumsAsync()
        {
            var albums = await _dbContext.Albums
                .Include(a => a.CreatedBy)
                .Include(a => a.Songs)
                .Select(a => new
                {
                    a.Id,
                    a.Name,
                    a.Artist,
                    a.ReleaseYear,
                    a.CoverUrl,
                    CreatedBy = a.CreatedBy != null ? a.CreatedBy.Email : "Unknown",
                    Songs = a.Songs.Select(s => new
                    {
                        s.Id,
                        s.Title,
                        s.Artist,
                        s.Year,
                        s.Genre,
                        s.FileUrl
                    }).ToList()
                })
                .ToListAsync();

            return albums.Cast<dynamic>().ToList();
        }

        public async Task<(bool Success, string? Error)> DeleteAlbumAsync(int albumId, string userId)
        {
            var album = await _dbContext.Albums
                .Include(a => a.Songs)
                .FirstOrDefaultAsync(a => a.Id == albumId);

            if (album == null)
                return (false, "Album not found");

            if (album.CreatedById != userId)
                return (false, "You can only delete your own albums");

            try
            {
                foreach (var song in album.Songs)
                    song.AlbumId = null;

                _dbContext.Albums.Remove(album);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
