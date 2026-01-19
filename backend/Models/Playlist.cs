using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Projekt_dotnet.Models
{
    public class Playlist
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Playlist name is required")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 200 characters")]
        public required string Name { get; set; } = null!;
        public bool IsPublic { get; set; } = false;
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // FK do użytkownika
        [Required(ErrorMessage = "User ID is required")]
        public required string UserId { get; set; } = null!;
        public IdentityUser? createdBy { get; set; } = null!;

        // Relacja wiele-do-wielu z utworami
        public ICollection<PlaylistSong> PlaylistSongs { get; set; } = new List<PlaylistSong>();
    }
}
