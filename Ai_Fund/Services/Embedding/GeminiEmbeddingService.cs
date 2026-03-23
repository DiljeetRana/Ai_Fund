using System.Text;
using System.Text.Json;

namespace Ai_Fund.Services.Embedding;

public class GeminiEmbeddingService : IEmbeddingService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _model;

    public GeminiEmbeddingService(IConfiguration configuration)
    {
        _httpClient = new HttpClient();
        _apiKey = (configuration["Gemini:ApiKey"] ?? "").Trim();
        var model = (configuration["Gemini:Model"] ?? "").Trim();
        
        if (string.IsNullOrEmpty(model))
        {
            model = "text-embedding-004"; // Fail-safe default
        }
        
        // Ensure models/ prefix
        _model = model.StartsWith("models/") ? model : $"models/{model}";

        if (string.IsNullOrEmpty(_apiKey))
        {
            throw new InvalidOperationException("Gemini API key not configured");
        }
    }

    public async Task<float[]> GenerateEmbeddingAsync(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return new float[768];

        var url = $"https://generativelanguage.googleapis.com/v1beta/{_model}:embedContent?key={_apiKey}";
        
        var payload = new
        {
            content = new { parts = new[] { new { text } } },
            output_dimensionality = 768
        };

        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);
        
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Gemini API Error ({response.StatusCode}): {errorContent}\nURL: {url.Replace(_apiKey, "REDACTED")}");
        }

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<JsonElement>(json);
        
        var values = result.GetProperty("embedding").GetProperty("values").EnumerateArray()
            .Select(x => (float)x.GetDouble()).ToArray();

        return values;
    }
}
