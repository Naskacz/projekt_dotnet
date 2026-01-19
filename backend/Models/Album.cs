using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace Projekt_dotnet.Models
{
    public class Album
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "An Album Name is required")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Album Name must be between 1 and 200 characters")]
        public required string Name { get; set; } = null!;
        [Required(ErrorMessage = "An Artist is required")]
        [StringLength(150, MinimumLength = 1, ErrorMessage = "Artist must be between 1 and 150 characters")]
        public required string Artist { get; set; } = null!;
        [Required(ErrorMessage = "Release Year is required")]
        [Range(1900, 2100, ErrorMessage = "Release Year must be between 1900 and 2100")]
        public required int ReleaseYear { get; set; }
        [Url(ErrorMessage = "CoverUrl must be a valid URL")]
        public string? CoverUrl { get; set; }
        [Required(ErrorMessage = "Creator ID is required")]
        public required string CreatedById { get; set; } = null!;
        public IdentityUser? CreatedBy { get; set; }
        public ICollection<Song> Songs { get; set; } = new List<Song>();
    }
}
