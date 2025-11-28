using Microsoft.AspNetCore.Identity;

namespace Projekt_dotnet.Models
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsPublic { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // FK do użytkownika
        public string UserId { get; set; } = null!;
        public IdentityUser? User { get; set; } = null!;

        // Relacja wiele-do-wielu z utworami
        public ICollection<PlaylistSong> PlaylistSongs { get; set; } = new List<PlaylistSong>();
    }
}
