using Microsoft.AspNetCore.Identity;


namespace Projekt_dotnet.Models
{
    public class Album
    {
        public int Id { get; set; }
        public required string Name { get; set; } = null!;
        public required string Artist { get; set; } = null!;
        public required int ReleaseYear { get; set; }
        public string? CoverUrl { get; set; }
        public IdentityUser? createdBy { get; set; }
        public ICollection<Song> Songs { get; set; } = new List<Song>();
    }
}
