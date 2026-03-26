using System.Text.Json.Serialization;

namespace Ai_Fund.Models;

public class MarketApiResponse
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("data")]
    public MarketDataDetails Data { get; set; } = new();
}

public class MarketDataDetails
{
    [JsonPropertyName("company_name")]
    public string CompanyName { get; set; } = string.Empty;

    [JsonPropertyName("last_price")]
    public double LastPrice { get; set; }

    [JsonPropertyName("change")]
    public double Change { get; set; }

    [JsonPropertyName("percent_change")]
    public double PercentChange { get; set; }
}
