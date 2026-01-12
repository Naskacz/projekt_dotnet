using Microsoft.EntityFrameworkCore;
using Projekt_dotnet.Data;
using Projekt_dotnet.Models;
using Projekt_dotnet.Models.DTOs;

namespace Projekt_dotnet.Services
{
    public interface IPlaylistService
    {
        Task<(bool Success, Playlist? Playlist, string? Error)> CreateAsync(CreatePlaylistDto dto, string userId);
        Task<List<Playlist>> GetMyAsync(string userId);
        Task<List<Playlist>> GetPublicAsync();
        Task<Playlist?> GetOneAsync(int id, string userId);
        Task<(bool Success, string? Error)> DeleteAsync(int id, string userId);
        Task<(bool Success, string? Error)> AddSongAsync(int playlistId, int songId, string userId);
        Task<(bool Success, string? Error)> RemoveSongAsync(int playlistId, int songId, string userId);
    }

    public class PlaylistService : IPlaylistService
    {
        private readonly ApplicationDbContext _db;

        public PlaylistService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<(bool Success, Playlist? Playlist, string? Error)> CreateAsync(CreatePlaylistDto dto, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return (false, null, "User not found");

            var playlist = new Playlist
            {
                Name = dto.Name,
                IsPublic = dto.IsPublic,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _db.Playlists.Add(playlist);
            await _db.SaveChangesAsync();
            return (true, playlist, null);
        }

        public async Task<List<Playlist>> GetMyAsync(string userId)
        {
            return await _db.Playlists
                .Where(p => p.UserId == userId)
                .Include(p => p.PlaylistSongs)
                .ThenInclude(ps => ps.Song)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Playlist>> GetPublicAsync()
        {
            return await _db.Playlists
                .Where(p => p.IsPublic)
                .Include(p => p.PlaylistSongs)
                .ThenInclude(ps => ps.Song)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<Playlist?> GetOneAsync(int id, string userId)
        {
            return await _db.Playlists
                .Include(p => p.PlaylistSongs.OrderBy(ps => ps.Order))
                .ThenInclude(ps => ps.Song)
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);
        }

        public async Task<(bool Success, string? Error)> DeleteAsync(int id, string userId)
        {
            var playlist = await _db.Playlists.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);
            if (playlist == null)
                return (false, "Playlist not found");

            _db.Playlists.Remove(playlist);
            await _db.SaveChangesAsync();
            return (true, null);
        }

        public async Task<(bool Success, string? Error)> AddSongAsync(int playlistId, int songId, string userId)
        {
            var playlist = await _db.Playlists
                .Include(p => p.PlaylistSongs)
                .FirstOrDefaultAsync(p => p.Id == playlistId && p.UserId == userId);

            if (playlist == null)
                return (false, "Playlist not found");

            var song = await _db.Songs.FirstOrDefaultAsync(s => s.Id == songId);
            if (song == null)
                return (false, "Song not found");

            var exists = playlist.PlaylistSongs.Any(ps => ps.SongId == songId);
            if (exists)
                return (false, "Song already in playlist");

            var order = playlist.PlaylistSongs.Count == 0 ? 1 : playlist.PlaylistSongs.Max(ps => ps.Order) + 1;
            playlist.PlaylistSongs.Add(new PlaylistSong
            {
                PlaylistId = playlistId,
                SongId = songId,
                Order = order
            });

            await _db.SaveChangesAsync();
            return (true, null);
        }

        public async Task<(bool Success, string? Error)> RemoveSongAsync(int playlistId, int songId, string userId)
        {
            var playlistSong = await _db.PlaylistSongs
                .Include(ps => ps.Playlist)
                .FirstOrDefaultAsync(ps => ps.PlaylistId == playlistId && ps.SongId == songId && ps.Playlist.UserId == userId);

            if (playlistSong == null)
                return (false, "Song not found in playlist");

            _db.PlaylistSongs.Remove(playlistSong);
            await _db.SaveChangesAsync();
            return (true, null);
        }
    }
}