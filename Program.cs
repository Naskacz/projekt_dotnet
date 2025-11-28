using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
// Add this using directive for Npgsql EF Core provider
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Projekt_dotnet.Data;
using Projekt_dotnet.Models;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("https://localhost:5001");

// Configure Kestrel to try to load certificate file from configuration/env and log on failure
builder.WebHost.ConfigureKestrel(options =>
{
    var config = builder.Configuration;

    var certPath = config["Kestrel:Certificates:Default:Path"]
                   ?? Environment.GetEnvironmentVariable("ASPNETCORE_Kestrel__Certificates__Default__Path");
    var certPassword = config["Kestrel:Certificates:Default:Password"]
                       ?? Environment.GetEnvironmentVariable("ASPNETCORE_Kestrel__Certificates__Default__Password");

    if (!string.IsNullOrEmpty(certPath) && File.Exists(certPath))
    {
        try
        {
            var cert = new X509Certificate2(certPath, certPassword);
            Console.WriteLine($"[Startup] Loaded certificate from {certPath}");
            options.ListenAnyIP(5001, listenOptions =>
            {
                listenOptions.UseHttps(cert);
            });
            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Startup] Failed to load certificate at '{certPath}': {ex.Message}");
            // fall through to default behavior below
        }
    }
    else
    {
        Console.WriteLine($"[Startup] Certificate path not set or file missing: '{certPath}'");
    }

    // fallback: ask Kestrel to use HTTPS (it will use default store/dev cert)
    options.ListenAnyIP(5001, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

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

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
