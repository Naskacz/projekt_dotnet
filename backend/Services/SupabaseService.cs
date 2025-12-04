using Supabase;
using Microsoft.Extensions.Configuration;

public class SupabaseService
{
    private readonly Client _client;
    private readonly string? _bucket;

    public SupabaseService(IConfiguration configuration)
    {
        var supabaseUrl = configuration["Supabase:Url"] ?? throw new InvalidOperationException("Supabase URL not configured");
        var supabaseKey = configuration["Supabase:ServiceKey"] ?? throw new InvalidOperationException("Supabase Service Key not configured");
        _bucket = configuration["Supabase:Bucket"];
        
        _client = new Client(supabaseUrl, supabaseKey);
    }

    public Client Client => _client;
    public string? Bucket => _bucket;
}