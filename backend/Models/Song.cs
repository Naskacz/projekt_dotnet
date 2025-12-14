using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Projekt_dotnet.Models
{
    public class Song
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "An Album Title is required")]
        public required string Title { get; set; } = null!;
        [Required(ErrorMessage = "An Artist is required")]
        public required string Artist { get; set; } = null!;
        public required string FileUrl { get; set; } = null!;
        public required string FileName {get; set; } = null!;
        public int? AlbumId { get; set; }
        public Album? Album { get; set; }
        public required int Year { get; set; }
        [Range(0, 2100, ErrorMessage = "Year must be valid")]
        public string? Genre { get; set; }
        public string? CreatedById { get; set; }
        public IdentityUser? CreatedBy { get; set; }
        // Navigation property for many-to-many relationship with Playlist
        public ICollection<PlaylistSong> PlaylistSongs{ get; set; } = new List<PlaylistSong>();
    }
}
