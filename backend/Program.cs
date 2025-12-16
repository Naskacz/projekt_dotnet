using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
// Add this using directive for Npgsql EF Core provider
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Projekt_dotnet.Data;
using Projekt_dotnet.Models;
using Projekt_dotnet.Services;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Configuration["Supabase:Url"] = Environment.GetEnvironmentVariable("SUPABASE_URL");
builder.Configuration["Supabase:ServiceKey"] = Environment.GetEnvironmentVariable("SUPABASE_SERVICE_KEY");
builder.Configuration["Supabase:Bucket"] = Environment.GetEnvironmentVariable("SUPABASE_BUCKET");

builder.Services.AddSingleton<SupabaseService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISongService, SongService>();
builder.Services.AddScoped<IAlbumService, AlbumService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Kestrel to try to load certificate file from configuration/env and log on failure

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllersWithViews();

builder.Services.AddCors(options => 
{
    options.AddPolicy("AllowLocalFrontend", policy =>
    {
        policy.WithOrigins("https://localhost:5173")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080); // HTTP na porcie 8080
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        NameClaimType = ClaimTypes.NameIdentifier
    };
});

builder.Services
    .AddIdentityCore<IdentityUser>(options =>
    {
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRouting();
app.UseCors("AllowLocalFrontend");

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
app.MapControllers();
app.Run();
