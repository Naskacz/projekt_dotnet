using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projekt_dotnet.Models;
using Projekt_dotnet.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

[ApiController]
[Route("api/[controller]")]

public class AlbumsController : ControllerBase {
    private readonly SupabaseService _supabaseService;

    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;

    public AlbumsController(SupabaseService supabaseService, ApplicationDbContext dbContext, UserManager<IdentityUser> userManager) {
        _dbContext = dbContext;
        _userManager = userManager;
        _supabaseService = supabaseService;
    }
    public class CreateAlbumDto {
        public required string Name { get; set; }
        public required string Artist { get; set; }
        public required int ReleaseYear { get; set; }
        public string? CoverUrl { get; set; }
    }
}