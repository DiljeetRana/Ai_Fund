namespace Ai_Fund.Data.Interfaces;

public interface IMutualFundRepository
{
    Task<List<(int Id, string Question, string Answer, string Embedding)>> GetAllKnowledgeAsync();
    Task UpdateEmbeddingAsync(int id, string embedding);
    Task<List<Models.ChatHistory>> GetChatHistoryAsync(string userId, int count = 5);
    Task SaveChatHistoryAsync(Models.ChatHistory chatHistory);
    Task SaveAiLogAsync(Models.AiLog aiLog);
}
