using Microsoft.EntityFrameworkCore;
using Projekt_dotnet.Data;
using Projekt_dotnet.Models;
using Projekt_dotnet.Models.DTOs;
using System.Security.Claims;

namespace Projekt_dotnet.Services
{
    public interface ISongService
    {
        Task<(bool Success, Song? Song, string? Error)> UploadSongAsync(CreateSongDto dto, string userId, SupabaseService supabaseService);
        Task<List<dynamic>> GetAllSongsAsync();
    }

    public class SongService : ISongService
    {
        private readonly ApplicationDbContext _dbContext;

        public SongService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(bool Success, Song? Song, string? Error)> UploadSongAsync(CreateSongDto dto, string userId, SupabaseService supabaseService)
        {
            if (string.IsNullOrEmpty(userId))
                return (false, null, "User not found");

            var userExists = await _dbContext.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
                return (false, null, "User ID from token does not exist in database");

            using var stream = dto.File.OpenReadStream();
            var uploadedUrl = await supabaseService.UploadSongAsync(stream, dto.File.FileName);

            if (string.IsNullOrWhiteSpace(uploadedUrl))
                return (false, null, "File upload failed");

            var song = new Song
            {
                Title = dto.Title,
                Artist = dto.Artist,
                Year = dto.Year,
                Genre = dto.Genre ?? "",
                FileName = dto.File.FileName,
                FileUrl = uploadedUrl,
                CreatedById = userId
            };

            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                _dbContext.Songs.Add(song);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return (true, song, null);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, null, ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<List<dynamic>> GetAllSongsAsync()
        {
            var songs = await _dbContext.Songs
                .Include(s => s.CreatedBy)
                .Select(s => new
                {
                    s.Id,
                    s.Title,
                    s.Artist,
                    s.Year,
                    s.Genre,
                    s.FileUrl,
                    s.FileName,
                    CreatedBy = s.CreatedBy != null ? s.CreatedBy.Email : "Unknown"
                })
                .ToListAsync();

            return songs.Cast<dynamic>().ToList();
        }
    }
}
