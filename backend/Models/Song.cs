using System.Collections.Generic;

namespace Projekt_dotnet.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Artist { get; set; } = null!;
        public string FileUrl { get; set; } = null!;
        public string FileName {get; set; } = null!;
        public int? AlbumId { get; set; }
        public Album? Album { get; set; }
        public int Year { get; set; }
        public string? Genre { get; set; }
        // Navigation property for many-to-many relationship with Playlist
        public ICollection<PlaylistSong> PlaylistSongs{ get; set; } = new List<PlaylistSong>();
    }
}
