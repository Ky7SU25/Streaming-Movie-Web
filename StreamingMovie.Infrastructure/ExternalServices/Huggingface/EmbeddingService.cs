using StreamingMovie.Application.Interfaces.ExternalServices.Huggingface;
using System.Text;
using System.Text.Json;

namespace StreamingMovie.Infrastructure.ExternalServices.Huggingface;

public class EmbeddingService : IEmbeddingService
{
    private readonly HttpClient _httpClient;

    public EmbeddingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<float[]> GetEmbeddingAsync(string text)
    {
        var content = new StringContent(JsonSerializer.Serialize(new { text }), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("http://127.0.0.1:5005/embed", content); // chỉnh lại URL nếu khác

        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        var json = JsonSerializer.Deserialize<Dictionary<string, float[]>>(responseString);

        return json["vector"];
    }
}
