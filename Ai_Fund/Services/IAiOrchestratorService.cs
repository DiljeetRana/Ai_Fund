using Ai_Fund.Models;

namespace Ai_Fund.Services;

public interface IAiOrchestratorService
{
    Task<ChatResponse> ProcessQueryAsync(string query, string userId);
}
