namespace Projekt_dotnet.Models.DTOs
{
    public class CreatePlaylistDto
    {
        public required string Name { get; set; }
        public bool IsPublic { get; set; } = false;
    }
}