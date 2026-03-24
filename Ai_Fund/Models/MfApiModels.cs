using System.Text.Json.Serialization;

namespace Ai_Fund.Models.MfApi;

public class MfSearchResponse
{
    [JsonPropertyName("schemeCode")]
    public int SchemeCode { get; set; }

    [JsonPropertyName("schemeName")]
    public string SchemeName { get; set; } = string.Empty;
}

public class MfNavData
{
    [JsonPropertyName("date")]
    public string Date { get; set; } = string.Empty;

    [JsonPropertyName("nav")]
    public string Nav { get; set; } = string.Empty;
}

public class MfMeta
{
    [JsonPropertyName("fund_house")]
    public string FundHouse { get; set; } = string.Empty;

    [JsonPropertyName("scheme_type")]
    public string SchemeType { get; set; } = string.Empty;

    [JsonPropertyName("scheme_category")]
    public string SchemeCategory { get; set; } = string.Empty;

    [JsonPropertyName("scheme_code")]
    public int SchemeCode { get; set; }

    [JsonPropertyName("scheme_name")]
    public string SchemeName { get; set; } = string.Empty;
}

public class MfSchemeResponse
{
    [JsonPropertyName("meta")]
    public MfMeta Meta { get; set; } = new();

    [JsonPropertyName("data")]
    public List<MfNavData> Data { get; set; } = new();

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
}

public class MfLatestNavResponse
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("data")]
    public List<MfNavData> Data { get; set; } = new();
}
