using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Ai_Fund.Services.Embedding;

public class NvidiaEmbeddingService : IEmbeddingService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _model;
    private readonly ILogger<NvidiaEmbeddingService> _logger;

    public NvidiaEmbeddingService(IConfiguration configuration, ILogger<NvidiaEmbeddingService> logger)
    {
        _httpClient = new HttpClient();
        _httpClient.Timeout = TimeSpan.FromSeconds(60); // NVIDIA models are large, giving more time
        
        _apiKey = configuration["Nvidia:ApiKey"]?.Trim() ?? string.Empty;
        _model = configuration["Nvidia:Model"] ?? "nvidia/nv-embed-v1";
        _logger = logger;

        if (string.IsNullOrEmpty(_apiKey))
        {
            _logger.LogError(">>> NVIDIA FAIL: API Key is NULL or EMPTY. Check Render Env Vars.");
        }
        else
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            _logger.LogInformation("NvidiaEmbeddingService initialized with model: {Model}", _model);
        }
    }

    public async Task<float[]> GenerateEmbeddingAsync(string text, string inputType = "query")
    {
        try
        {
            var keyHint = string.IsNullOrEmpty(_apiKey) ? "MISSING" : _apiKey.Length > 5 ? _apiKey.Substring(0, 5) + "..." : "TOO_SHORT";
            _logger.LogInformation(">>> NVIDIA START: Model={Model}, Type={Type}, KeyHint={KeyHint}", _model, inputType, keyHint);

            if (string.IsNullOrEmpty(_apiKey))
            {
                return new float[4096]; // Savage fail-safe
            }

            // NVIDIA nv-embed-v1 requires input_type, input, and model
            var payload = new
            {
                input = text,
                model = _model,
                input_type = inputType,
                encoding_format = "float",
                truncate = "NONE"
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower };
            
            _logger.LogInformation(">>> NVIDIA PAYLOAD: Model={Model}, Type={Type}", _model, inputType);

            var response = await _httpClient.PostAsJsonAsync("https://integrate.api.nvidia.com/v1/embeddings", payload, options);
            var content = await response.Content.ReadAsStringAsync();
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(">>> NVIDIA FAIL: Status {Status}. Response: {Content}", response.StatusCode, content);
                return new float[4096]; 
            }

            var result = JsonSerializer.Deserialize<NvidiaResponse>(content, new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            });

            if (result?.Data == null || result.Data.Count == 0 || result.Data[0].Embedding == null)
            {
                _logger.LogWarning(">>> NVIDIA FAIL: Malformed JSON or empty data. Content: {Content}", content);
                return new float[4096];
            }

            var embedding = result.Data[0].Embedding;
            _logger.LogInformation(">>> NVIDIA SUCCESS: Generated {Count}d embedding", embedding.Length);
            
            return embedding;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ">>> NVIDIA EXCEPTION: {Message}", ex.Message);
            return new float[4096];
        }
    }
}

public class NvidiaResponse
{
    public List<NvidiaData>? Data { get; set; }
}

public class NvidiaData
{
    public float[]? Embedding { get; set; }
    public int Index { get; set; }
}
