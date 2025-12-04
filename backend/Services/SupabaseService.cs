using Supabase;
using Supabase.Storage;
using Microsoft.Extensions.Configuration;
using SupabaseClient = Supabase.Client;
using SupabaseStorageClient = Supabase.Storage.Client;

public class SupabaseService
{
    private readonly SupabaseClient _client;
    private readonly string _bucket;

    public SupabaseService(IConfiguration configuration)
    {
        var supabaseUrl = configuration["Supabase:Url"] 
            ?? throw new InvalidOperationException("Supabase URL not configured");

        var supabaseKey = configuration["Supabase:ServiceKey"] 
            ?? throw new InvalidOperationException("Supabase Service Key not configured");

        _bucket = configuration["Supabase:Bucket"] 
            ?? throw new InvalidOperationException("Supabase bucket not configured");

        _client = new SupabaseClient(supabaseUrl, supabaseKey, new SupabaseOptions
        {
            AutoConnectRealtime = false
        });
    }

    public async Task<string> UploadSongAsync(Stream fileStream, string fileName)
    {
        await _client.InitializeAsync();

        var storage = _client.Storage;
        var bucket = storage.From(_bucket);

        var path = $"songs/{Guid.NewGuid()}_{fileName}";

        // Konwersja Stream → byte[]
        byte[] bytes;
        using (var ms = new MemoryStream())
        {
            await fileStream.CopyToAsync(ms);
            bytes = ms.ToArray();
        }

        await bucket.Upload(bytes, path, new Supabase.Storage.FileOptions
        {
            CacheControl = "3600",
            Upsert = false
        });

        return bucket.GetPublicUrl(path);
    }


    public SupabaseClient Client => _client;
    public string Bucket => _bucket;
}
