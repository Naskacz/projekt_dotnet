namespace Projekt_dotnet.Models.DTOs
{
    public class CreateAlbumDto
    {
        public required string Name { get; set; }
        public required string Artist { get; set; }
        public required int ReleaseYear { get; set; }
        public string? CoverUrl { get; set; }
    }
}
