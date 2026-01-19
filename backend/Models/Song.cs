using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Projekt_dotnet.Models
{
    public class Song
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "A Song Title is required")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters")]
        public required string Title { get; set; } = null!;
        [Required(ErrorMessage = "An Artist is required")]
        [StringLength(150, MinimumLength = 1, ErrorMessage = "Artist must be between 1 and 150 characters")]
        public required string Artist { get; set; } = null!;
        [Required(ErrorMessage = "A File URL is required")]
        [Url(ErrorMessage = "FileUrl must be a valid URL")]
        public required string FileUrl { get; set; } = null!;
        [Required(ErrorMessage = "A File Name is required")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "FileName must be between 1 and 255 characters")]
        public required string FileName { get; set; } = null!;
        public int? AlbumId { get; set; }
        public Album? Album { get; set; }
        [Required(ErrorMessage = "Year is required")]
        [Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100")]
        public required int Year { get; set; }
        [StringLength(50, ErrorMessage = "Genre cannot exceed 50 characters")]
        public string? Genre { get; set; }
        [Required(ErrorMessage = "Creator ID is required")]
        public required string CreatedById { get; set; } = null!;
        public IdentityUser? CreatedBy { get; set; }
        // Navigation property for many-to-many relationship with Playlist
        public ICollection<PlaylistSong> PlaylistSongs { get; set; } = new List<PlaylistSong>();
    }
}
