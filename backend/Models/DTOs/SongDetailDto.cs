namespace Projekt_dotnet.Models.DTOs
{
    public class SongDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Artist { get; set; } = "";
        public int? Year { get; set; }
        public string Genre { get; set; } = "";
        public string FileUrl { get; set; } = "";
        public AlbumShortDto? Album { get; set; }
    }

    public class AlbumShortDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Artist { get; set; } = "";
        public int ReleaseYear { get; set; }
        public string? CoverUrl { get; set; }
    }
}