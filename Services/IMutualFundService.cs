namespace Ai_Fund.Services;

public interface IMutualFundService
{
    Task<string> GetAnswerAsync(string query);
    Task<Models.ChatResponse> GetAIAnswerAsync(string query);
}
