using Ai_Fund.Models;

namespace Ai_Fund.Services;

public class OllamaLLMService : ILLMService
{
    private readonly HttpClient _httpClient;
    private readonly string _ollamaEndpoint;
    private readonly string _model;

    public OllamaLLMService(IConfiguration configuration)
    {
        _httpClient = new HttpClient();
        _httpClient.Timeout = TimeSpan.FromMinutes(2);
        _ollamaEndpoint = configuration["Ollama:Endpoint"] ?? "http://localhost:11434";
        _model = configuration["Ollama:Model"] ?? "tinyllama";
    }

    public async Task<string> AskLLMAsync(string context, string query, List<ChatMessage> chatHistory)
    {
        var historyText = string.Join("\n", chatHistory
            .Select(x => $"{x.Role}: {x.Content}"));

        var prompt = $@"
You are a financial assistant.

STRICT RULES:
- Answer ONLY from the provided context
- DO NOT change terms (e.g., SIP must remain SIP)
- DO NOT repeat full explanations unless asked
- Answer ONLY what user asked
- If question is follow-up: use previous context and answer directly
- DO NOT give advice like ""ask advisor""
- DO NOT add extra information
- Keep answer short (2-4 lines)
- Answer in maximum 3 lines
- If answer not found: say ""I don't have enough information""

Context:
{context}

Conversation:
{historyText}

Question:
{query}
";

        var request = new
        {
            model = _model,
            prompt = prompt,
            stream = false
        };

        var response = await _httpClient.PostAsJsonAsync($"{_ollamaEndpoint}/api/generate", request);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<OllamaResponse>();

        return result?.response ?? "I couldn't generate a response.";
    }
}
