using Ai_Fund.Data.Interfaces;
using Ai_Fund.Services.Embedding;
using Ai_Fund.Models;
using System.Text.Json;

namespace Ai_Fund.Services;

public class MutualFundService : IMutualFundService
{
    private readonly IMutualFundRepository _repository;
    private readonly IEmbeddingService _embeddingService;
    private readonly ILLMService _llmService;
    private static List<ChatMessage> _chatHistory = new List<ChatMessage>();

    public MutualFundService(IMutualFundRepository repository, IEmbeddingService embeddingService, ILLMService llmService)
    {
        _repository = repository;
        _embeddingService = embeddingService;
        _llmService = llmService;
    }

    public async Task<string> GetAnswerAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return "Please provide a valid query";

        // Generate embedding for the query
        var queryEmbedding = await _embeddingService.GenerateEmbeddingAsync(query);
        
        // Get all knowledge from database
        var allData = await _repository.GetAllKnowledgeAsync();

        // Find best match using cosine similarity
        var bestMatch = allData
            .Where(x => !string.IsNullOrEmpty(x.Embedding))
            .Select(x => new
            {
                Data = x,
                Score = VectorHelper.CosineSimilarity(
                    queryEmbedding,
                    JsonSerializer.Deserialize<float[]>(x.Embedding) ?? Array.Empty<float>()
                )
            })
            .OrderByDescending(x => x.Score)
            .FirstOrDefault();

        if (bestMatch == null || bestMatch.Score < 0.7)
            return "I don't have enough information.";

        return bestMatch.Data.Answer;
    }

    public async Task<Models.ChatResponse> GetAIAnswerAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return new Models.ChatResponse
            {
                Answer = "Please provide a valid query",
                Source = "System",
                Confidence = 0,
                Intent = "INVALID"
            };

        // Detect intent (before normalization for better detection)
        var intent = IntentDetector.DetectIntent(query);

        // Handle greetings
        if (intent == "GREETING")
        {
            return new Models.ChatResponse
            {
                Answer = "Hello! I can help you with mutual fund queries.",
                Source = "Static",
                Confidence = 1.0,
                Intent = intent
            };
        }

        // Handle closing
        if (intent == "CLOSING")
        {
            return new Models.ChatResponse
            {
                Answer = "Thank you for using our service. Have a great day!",
                Source = "Static",
                Confidence = 1.0,
                Intent = intent
            };
        }

        // Handle advice requests
        if (intent == "ADVICE")
        {
            return new Models.ChatResponse
            {
                Answer = "I can provide general information, but I cannot give personalized financial advice.",
                Source = "Static",
                Confidence = 1.0,
                Intent = intent
            };
        }

        // Save user message
        _chatHistory.Add(new ChatMessage { Role = "User", Content = query });

        // Normalize query for embedding
        var normalizedQuery = TextNormalizer.Normalize(query);

        // Generate embedding for the normalized query
        var queryEmbedding = await _embeddingService.GenerateEmbeddingAsync(normalizedQuery);
        
        // Get all knowledge from database
        var allData = await _repository.GetAllKnowledgeAsync();

        // Find top 3 matches using cosine similarity
        var topMatches = allData
            .Where(x => !string.IsNullOrEmpty(x.Embedding))
            .Select(x =>
            {
                try
                {
                    var embedding = JsonSerializer.Deserialize<float[]>(x.Embedding);
                    if (embedding == null || embedding.Length == 0)
                        return null;
                    
                    return new
                    {
                        Data = x,
                        Score = VectorHelper.CosineSimilarity(queryEmbedding, embedding)
                    };
                }
                catch
                {
                    return null;
                }
            })
            .Where(x => x != null && x.Score > 0.6)
            .OrderByDescending(x => x.Score)
            .Take(3)
            .ToList();

        if (!topMatches.Any())
        {
            var noInfoResponse = "I don't have enough information.";
            _chatHistory.Add(new ChatMessage { Role = "Assistant", Content = noInfoResponse });
            
            // Keep only last 5 messages
            if (_chatHistory.Count > 5)
                _chatHistory.RemoveAt(0);
            
            return new Models.ChatResponse
            {
                Answer = noInfoResponse,
                Source = "System",
                Confidence = 0,
                Intent = intent
            };
        }

        // Combine top 3 contexts (distinct)
        var context = string.Join("\n",
            topMatches
                .Select(x => x.Data.Answer)
                .Distinct());

        // For simple definitions, return direct answer
        if (intent == "DEFINITION" && topMatches[0].Score > 0.8)
        {
            var directAnswer = topMatches[0].Data.Answer;
            _chatHistory.Add(new ChatMessage { Role = "Assistant", Content = directAnswer });
            
            // Keep only last 5 messages
            if (_chatHistory.Count > 5)
                _chatHistory.RemoveAt(0);
            
            return new Models.ChatResponse
            {
                Answer = directAnswer,
                Source = "Database",
                Confidence = topMatches[0].Score,
                Intent = intent
            };
        }

        // Generate AI response using LLM with chat history
        var aiResponse = await _llmService.AskLLMAsync(context, query, _chatHistory);

        // Enhanced safety filter
        if (aiResponse.Contains("guarantee", StringComparison.OrdinalIgnoreCase) || 
            aiResponse.Contains("fixed return", StringComparison.OrdinalIgnoreCase) ||
            aiResponse.Contains("$") ||
            aiResponse.Contains("₹"))
        {
            return new Models.ChatResponse
            {
                Answer = "I can provide general information, but not financial advice.",
                Source = "SafetyFilter",
                Confidence = 0,
                Intent = "BLOCKED"
            };
        }

        // Save bot response
        _chatHistory.Add(new ChatMessage { Role = "Assistant", Content = aiResponse });

        // Keep only last 5 messages
        if (_chatHistory.Count > 5)
            _chatHistory.RemoveAt(0);

        return new Models.ChatResponse
        {
            Answer = aiResponse,
            Source = "RAG+LLM",
            Confidence = topMatches[0].Score,
            Intent = intent
        };
    }
}
