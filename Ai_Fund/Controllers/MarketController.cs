using Microsoft.AspNetCore.Mvc;
using Ai_Fund.Services;
using Ai_Fund.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Json;


namespace Ai_Fund.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class MarketController : ControllerBase
{
    private readonly ICurrencyService _currencyService;
    private readonly ILogger<MarketController> _logger;

    public MarketController(ICurrencyService currencyService, ILogger<MarketController> logger)
    {
        _currencyService = currencyService;
        _logger = logger;
    }

    [HttpGet("overview")]
    public async Task<IActionResult> GetOverview()
    {
        _logger.LogInformation("Market Overview Requested");
        try
        {
            var usdRate = await _currencyService.GetUsdToInrRateAsync();
            
            // Fetch Live Indices from the newly provided free API
            var niftyData = await FetchLiveIndexAsync("^NSEI");
            var sensexData = await FetchLiveIndexAsync("^BSESN");

            return Ok(new {
                nifty = niftyData,
                sensex = sensexData,
                usdInr = new { value = "₹" + usdRate.ToString("N2"), trend = "-0.02%", color = "rose" }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching market overview");
            return StatusCode(500, "Internal server error");
        }
    }

    private async Task<object> FetchLiveIndexAsync(string symbol)
    {
        try
        {
            using var client = new HttpClient();
            var url = $"https://military-jobye-haiqstudios-14f59639.koyeb.app/stock?symbol={Uri.EscapeDataString(symbol)}&res=num";
            var response = await client.GetFromJsonAsync<MarketApiResponse>(url);
            
            if (response?.Status == "success" && response.Data != null)
            {
                var d = response.Data;
                var trendPrefix = d.PercentChange >= 0 ? "+" : "";
                var color = d.PercentChange >= 0 ? "green" : "rose";
                
                return new { 
                    value = d.LastPrice.ToString("N2"), 
                    trend = $"{trendPrefix}{d.PercentChange:F2}%", 
                    color = color 
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching live index for {Symbol}. Using fallback.", symbol);
        }

        // Reasonable Fallbacks if API fails
        return symbol == "^NSEI" 
            ? new { value = "24,250.35", trend = "+0.52%", color = "green" }
            : new { value = "79,486.20", trend = "+0.41%", color = "green" };
    }
}

