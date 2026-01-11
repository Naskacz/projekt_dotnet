public class AlbumDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Artist { get; set; }
    public int ReleaseYear { get; set; }
    public string? CoverUrl { get; set; }
    public string CreatedBy { get; set; }
    public List<SongDto> Songs { get; set; }
}

public class SongDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Artist { get; set; }
    public int? Year { get; set; }
    public string Genre { get; set; }
    public string FileUrl { get; set; }
}