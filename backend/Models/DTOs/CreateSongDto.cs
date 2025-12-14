namespace Projekt_dotnet.Models.DTOs
{
    public class CreateSongDto
    {
        public required IFormFile File { get; set; }
        public required string Title { get; set; }
        public required string Artist { get; set; }
        public required int Year { get; set; }
        public required string Genre { get; set; }
    }
}
