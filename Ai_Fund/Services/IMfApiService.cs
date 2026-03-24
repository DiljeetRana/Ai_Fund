using Ai_Fund.Models.MfApi;

namespace Ai_Fund.Services;

public interface IMfApiService
{
    Task<List<MfSearchResponse>> SearchSchemesAsync(string query);
    Task<MfSchemeResponse?> GetSchemeDetailsAsync(int schemeCode);
    Task<MfNavData?> GetLatestNavAsync(int schemeCode);
}
