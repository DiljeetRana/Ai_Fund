using System.Text.Json.Serialization;

namespace Ai_Fund.Models;

public class MarketAuxResponse
{
    [JsonPropertyName("data")]
    public List<MarketAuxArticle> Data { get; set; } = new();
}

public class MarketAuxArticle
{
    [JsonPropertyName("uuid")]
    public string Uuid { get; set; } = string.Empty;

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("snippet")]
    public string Snippet { get; set; } = string.Empty;

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("published_at")]
    public DateTimeOffset? PublishedAt { get; set; }

    [JsonPropertyName("source")]
    public string Source { get; set; } = string.Empty;
}
