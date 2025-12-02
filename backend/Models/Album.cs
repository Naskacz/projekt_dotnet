namespace Projekt_dotnet.Models
{
    public class Album
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Artist { get; set; } = null!;
        public int ReleaseYear { get; set; }
        public string? CoverUrl { get; set; }
        public ICollection<Song> Songs { get; set; } = new List<Song>();
    }
}
