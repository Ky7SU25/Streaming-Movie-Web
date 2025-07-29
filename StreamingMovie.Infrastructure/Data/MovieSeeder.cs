using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;


namespace StreamingMovie.Infrastructure.Data;

public class MovieSeeder
{
    private readonly MovieDbContext _context;
    private readonly HttpClient _httpClient;

    public MovieSeeder(MovieDbContext context, HttpClient httpClient)
    {
        _context = context;
        _httpClient = httpClient;
    }

    public async Task SeedAsync()
    {
        var moviesWithoutEmbedding = _context.Movies
            .Where(m => string.IsNullOrEmpty(m.EmbeddingJson))
            .Include(m => m.MovieCategories)
                .ThenInclude(mc => mc.Category)
            .Include(m => m.MovieActors)
                .ThenInclude(ma => ma.Actor)
            .Include(m => m.MovieDirectors)
                .ThenInclude(md => md.Director)
            .Include(m => m.Country)
            .ToList();

        if (!moviesWithoutEmbedding.Any())
        {
            Console.WriteLine("✅ All movies already have embeddings.");
            return;
        }

        foreach (var movie in moviesWithoutEmbedding)
        {
            try
            {
                var categories = string.Join(", ", movie.MovieCategories.Select(mc => mc.Category.Name));
                var actors = string.Join(", ", movie.MovieActors.Select(ma => ma.Actor.Name));
                var directors = string.Join(", ", movie.MovieDirectors.Select(md => md.Director.Name));
                var country = movie.Country?.ToString() ?? "";

                var combinedText = $"{movie.Title}. {movie.Description}. " +
                                   $"Categories: {categories}. " +
                                   $"Actors: {actors}. " +
                                   $"Directors: {directors}. " +
                                   $"Country: {country}.";

                var embedding = await GetEmbeddingAsync(combinedText);
                movie.Embedding = embedding;
                Console.WriteLine($"✅ Generated embedding for movie: {movie.Title}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to generate embedding for movie: {movie.Title}. Error: {ex.Message}");
            }
        }

        await _context.SaveChangesAsync();
        Console.WriteLine("✅ Embedding update completed.");
    }



    private async Task<float[]> GetEmbeddingAsync(string text)
    {
        var content = new StringContent(JsonSerializer.Serialize(new { text }), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("http://127.0.0.1:5005/embed", content);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();

        Console.WriteLine("Raw JSON from Flask:");
        Console.WriteLine(json);

        var result = JsonSerializer.Deserialize<Dictionary<string, float[]>>(json);

        if (result != null && result.ContainsKey("vector"))
            return result["vector"]; // ✅ Đổi từ "embedding" → "vector"
        else
            throw new Exception("Response JSON does not contain 'vector' key.");
    }

}
