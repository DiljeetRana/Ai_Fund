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

        var prompt = $@"You are a professional financial assistant helping users understand mutual funds.

Your Role:
- Provide accurate, helpful information based only on the given context
- Answer in a clear, professional, and friendly manner
- Keep responses concise (2-4 lines maximum)

Strict Rules:
1. Answer ONLY from the provided context below
2. Do NOT change financial terms (e.g., SIP must remain SIP)
3. Do NOT add information not in the context
4. Do NOT give personalized financial advice
5. If the question is a follow-up, use conversation history for context
6. Keep all financial safety disclaimers
7. Maximum 3 sentences in your response

Context:
{context}

Conversation History:
{historyText}

User Question:
{query}

Your Answer (2-3 sentences):";

        var request = new
        {
            model = _model,
            prompt = prompt,
            stream = false,
            options = new
            {
                temperature = 0.3,
                top_p = 0.9
            }
        };

        var response = await _httpClient.PostAsJsonAsync($"{_ollamaEndpoint}/api/generate", request);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<OllamaResponse>();

        return result?.response ?? "I couldn't generate a response.";
    }

    public async Task<string> RewriteAnswerAsync(string answer, string query)
    {
        var prompt = $@"You are a professional financial content writer.

Task: Rewrite the answer below in a clear, professional, and conversational tone.

Guidelines:
1. Keep the exact same information - do not add or remove facts
2. Maintain all financial disclaimers and safety statements
3. Use 2-3 sentences maximum
4. Write in simple, friendly language
5. Be professional but not robotic
6. Do not use slang or overly casual language

Original Answer:
{answer}

Your Rewritten Answer (2-3 sentences only):";

        var request = new
        {
            model = _model,
            prompt = prompt,
            stream = false,
            options = new
            {
                temperature = 0.3,
                top_p = 0.9
            }
        };

        var response = await _httpClient.PostAsJsonAsync($"{_ollamaEndpoint}/api/generate", request);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<OllamaResponse>();

        return result?.response ?? answer;
    }
}
