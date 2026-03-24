using Ai_Fund.Models.MfApi;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace Ai_Fund.Services;

public class MfApiService : IMfApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<MfApiService> _logger;

    public MfApiService(HttpClient httpClient, ILogger<MfApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _httpClient.BaseAddress = new Uri("https://api.mfapi.in");
    }

    public async Task<List<MfSearchResponse>> SearchSchemesAsync(string query)
    {
        try
        {
            _logger.LogInformation("Searching for mutual fund schemes with query: {Query}", query);
            var response = await _httpClient.GetFromJsonAsync<List<MfSearchResponse>>($"/mf/search?q={query}");
            return response ?? new List<MfSearchResponse>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching for mutual fund schemes with query: {Query}", query);
            return new List<MfSearchResponse>();
        }
    }

    public async Task<MfSchemeResponse?> GetSchemeDetailsAsync(int schemeCode)
    {
        try
        {
            _logger.LogInformation("Fetching scheme details for scheme code: {SchemeCode}", schemeCode);
            return await _httpClient.GetFromJsonAsync<MfSchemeResponse>($"/mf/{schemeCode}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching scheme details for scheme code: {SchemeCode}", schemeCode);
            return null;
        }
    }

    public async Task<MfNavData?> GetLatestNavAsync(int schemeCode)
    {
        try
        {
            _logger.LogInformation("Fetching latest NAV for scheme code: {SchemeCode}", schemeCode);
            var response = await GetSchemeDetailsAsync(schemeCode);
            return response?.Data?.FirstOrDefault();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching latest NAV for scheme code: {SchemeCode}", schemeCode);
            return null;
        }
    }
}
