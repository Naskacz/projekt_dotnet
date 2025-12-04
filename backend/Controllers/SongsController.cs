using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SongsController : ControllerBase
{
    private readonly SupabaseService _supabaseService;

    public SongsController(SupabaseService supabaseService)
    {
        _supabaseService = supabaseService;
    }

    [HttpPost("upload")]
    [RequestSizeLimit(50_000_000)]
    public async Task<IActionResult> UploadSong(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        using var stream = file.OpenReadStream();
        var url = await _supabaseService.UploadSongAsync(stream, file.FileName);

        return Ok(new { url });
    }
}

// Extension to provide UploadSongAsync so the controller compiles; replace with real implementation as needed.
public static class SupabaseServiceExtensions
{
    public static Task<string> UploadSongAsync(this SupabaseService supabaseService, Stream stream, string fileName)
    {
        // Placeholder implementation returning a fake URL so the project builds;
        // Replace with real upload logic (e.g., call into Supabase storage SDK) in production.
        return Task.FromResult($"https://example.com/{fileName}");
    }
}

