using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Ai_Fund.Models;

namespace Ai_Fund.Services;

public class MultiSourceMarketNewsService : IMarketNewsService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<MultiSourceMarketNewsService> _logger;
    private readonly IConfiguration _configuration;

    public MultiSourceMarketNewsService(
        HttpClient httpClient,
        ILogger<MultiSourceMarketNewsService> logger,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _configuration = configuration;
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36");
    }

    public bool IsLiveMarketQuery(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return false;
        }

        var q = query.ToLowerInvariant();
        var keywords = new[]
        {
            "market", "news", "headline", "headlines", "nifty", "sensex", "stocks", "share market",
            "falling", "down today", "war", "conflict", "attack", "missile", "usa", "us ", "america",
            "russia", "ukraine", "israel", "iran", "china", "tariff", "oil", "crude", "fed", "recession"
        };

        return keywords.Any(q.Contains);
    }

    public async Task<List<YahooArticle>> GetLatestMarketNewsAsync(string query)
    {
        var marketAuxTask = GetMarketAuxArticlesAsync(query);
        var yahooTask = GetYahooArticlesAsync(query);

        await Task.WhenAll(marketAuxTask, yahooTask);

        var combined = marketAuxTask.Result
            .Concat(yahooTask.Result)
            .Where(article => !string.IsNullOrWhiteSpace(article.Title) && !string.IsNullOrWhiteSpace(article.Url))
            .GroupBy(article => article.Url, StringComparer.OrdinalIgnoreCase)
            .Select(group => group.First())
            .OrderByDescending(article => ParsePublishedAt(article.PublishedAt))
            .ThenBy(article => article.Provider != "MarketAux")
            .Take(6)
            .ToList();

        return combined;
    }

    public string BuildNewsContext(List<YahooArticle> articles, string query)
    {
        if (articles == null || !articles.Any())
        {
            return string.Empty;
        }

        var lines = articles.Select(article =>
        {
            var published = string.IsNullOrWhiteSpace(article.PublishedAt)
                ? string.Empty
                : $" | Published: {article.PublishedAt}";
            var description = string.IsNullOrWhiteSpace(article.Description)
                ? string.Empty
                : $" | Summary: {article.Description}";

            return $"- {article.Title} (Source: {article.Source}, Provider: {article.Provider}{published}){description}";
        });

        return $"\n\nLATEST NEWS CONTEXT FOR \"{query}\":\n{string.Join("\n", lines)}";
    }

    private async Task<List<YahooArticle>> GetMarketAuxArticlesAsync(string query)
    {
        var apiKey = _configuration["MarketAux:ApiKey"];
        var baseUrl = _configuration["MarketAux:BaseUrl"]?.TrimEnd('/') ?? "https://api.marketaux.com";
        var language = _configuration["MarketAux:Language"] ?? "en";
        var limit = _configuration["MarketAux:Limit"] ?? "5";

        if (string.IsNullOrWhiteSpace(apiKey) || apiKey.Contains("YOUR_MARKETAUX_API_KEY_HERE", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogInformation("MarketAux API key is not configured. Skipping MarketAux news fetch.");
            return new List<YahooArticle>();
        }

        try
        {
            var search = Uri.EscapeDataString(BuildSearchQuery(query));
            var url = $"{baseUrl}/v1/news/all?api_token={Uri.EscapeDataString(apiKey)}&search={search}&language={Uri.EscapeDataString(language)}&limit={Uri.EscapeDataString(limit)}&sort=published_desc";

            if (ShouldUseIndiaFilter(query))
            {
                var countries = _configuration["MarketAux:Countries"];
                if (!string.IsNullOrWhiteSpace(countries))
                {
                    url += $"&countries={Uri.EscapeDataString(countries)}";
                }
            }

            var result = await _httpClient.GetFromJsonAsync<MarketAuxNewsResponse>(url);

            return result?.Data?
                .Where(item => !string.IsNullOrWhiteSpace(item.Url))
                .Select(item => new YahooArticle
                {
                    Uuid = item.Uuid,
                    Title = item.Title ?? string.Empty,
                    Description = item.Description ?? string.Empty,
                    Url = item.Url ?? string.Empty,
                    Source = item.Source ?? "MarketAux",
                    ImageUrl = item.ImageUrl ?? string.Empty,
                    PublishedAt = item.PublishedAt ?? string.Empty,
                    Provider = "MarketAux"
                })
                .ToList() ?? new List<YahooArticle>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching MarketAux news for query {Query}", query);
            return new List<YahooArticle>();
        }
    }

    private async Task<List<YahooArticle>> GetYahooArticlesAsync(string query)
    {
        try
        {
            var yahooQuery = Uri.EscapeDataString(BuildSearchQuery(query));
            var url = $"https://query2.finance.yahoo.com/v1/finance/search?q={yahooQuery}&newsCount=4";
            var result = await _httpClient.GetFromJsonAsync<YahooSearchResponse>(url);

            return result?.News?
                .Select(item => new YahooArticle
                {
                    Uuid = item.Uuid,
                    Title = item.Title,
                    Description = string.Empty,
                    Url = item.Link,
                    Source = item.Publisher,
                    ImageUrl = item.Thumbnail?.Resolutions?.FirstOrDefault()?.Url ?? string.Empty,
                    PublishedAt = item.ProviderPublishTime > 0
                        ? DateTimeOffset.FromUnixTimeSeconds(item.ProviderPublishTime).UtcDateTime.ToString("yyyy-MM-ddTHH:mm:ssZ")
                        : string.Empty,
                    Provider = "Yahoo"
                })
                .ToList() ?? new List<YahooArticle>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching Yahoo news for query {Query}", query);
            return new List<YahooArticle>();
        }
    }

    private static bool ShouldUseIndiaFilter(string query)
    {
        var q = query.ToLowerInvariant();
        return q.Contains("india") || q.Contains("indian") || q.Contains("nifty") || q.Contains("sensex");
    }

    private static string BuildSearchQuery(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return "market news";
        }

        if (query.Contains("war", StringComparison.OrdinalIgnoreCase) ||
            query.Contains("conflict", StringComparison.OrdinalIgnoreCase))
        {
            return $"{query} stock market impact";
        }

        return query;
    }

    private static DateTime ParsePublishedAt(string value)
    {
        return DateTime.TryParse(value, out var parsed) ? parsed : DateTime.MinValue;
    }
}

public class MarketAuxNewsResponse
{
    [JsonPropertyName("data")]
    public List<MarketAuxNewsItem> Data { get; set; } = new();
}

public class MarketAuxNewsItem
{
    [JsonPropertyName("uuid")]
    public string Uuid { get; set; } = string.Empty;

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("source")]
    public string? Source { get; set; }

    [JsonPropertyName("image_url")]
    public string? ImageUrl { get; set; }

    [JsonPropertyName("published_at")]
    public string? PublishedAt { get; set; }
}
