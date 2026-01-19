using System.ComponentModel.DataAnnotations;


namespace Projekt_dotnet.Models
{
    public class PlaylistSong
    {
        [Required(ErrorMessage = "Playlist ID is required")]
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; } = null!;
        [Required(ErrorMessage = "Song ID is required")]
        public int SongId { get; set; }
        public Song Song { get; set; } = null!;
        [Range(1, int.MaxValue, ErrorMessage = "Order must be a positive number")]
        public int Order { get; set; }
    }
}
