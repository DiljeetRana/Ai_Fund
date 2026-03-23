using Ai_Fund.Data.Interfaces;
using Ai_Fund.Services.Embedding;
using System.Text.Json;

namespace Ai_Fund.Services;

public interface ISyncService
{
    Task SyncKnowledgeToQdrantAsync();
}

public class SyncService : ISyncService
{
    private readonly IMutualFundRepository _repository;
    private readonly IEmbeddingService _embeddingService;
    private readonly IQdrantService _qdrantService;
    private readonly ILogger<SyncService> _logger;

    public SyncService(
        IMutualFundRepository repository,
        IEmbeddingService embeddingService,
        IQdrantService qdrantService,
        ILogger<SyncService> logger)
    {
        _repository = repository;
        _embeddingService = embeddingService;
        _qdrantService = qdrantService;
        _logger = logger;
    }

    public async Task SyncKnowledgeToQdrantAsync()
    {
        try
        {
            _logger.LogInformation("Starting sync from SQL to Qdrant...");
            _logger.LogInformation(">>> MIGRATION: Deleting and recreating collection for 768-dimension (Gemini AI) change...");

            // Delete existing collection to change dimensions
            await _qdrantService.DeleteCollectionAsync();

            // Ensure collection exists (with new size)
            await _qdrantService.InitializeCollectionAsync();

            // Get all active knowledge from SQL
            var allKnowledge = await _repository.GetAllKnowledgeAsync();

            _logger.LogInformation("Found {Count} active knowledge entries to sync", allKnowledge.Count);

            int syncedCount = 0;
            int skippedCount = 0;

            var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 10 };
            
            await Parallel.ForEachAsync(allKnowledge, parallelOptions, async (item, ct) =>
            {
                try
                {
                    // Skip if content is empty
                    if (string.IsNullOrWhiteSpace(item.Answer))
                    {
                        Interlocked.Increment(ref skippedCount);
                        return;
                    }

                    // Generate Gemini embedding (768d)
                    var normalizedQuestion = TextNormalizer.Normalize(item.Question);
                    var embedding = await _embeddingService.GenerateEmbeddingAsync(normalizedQuestion);
                    
                    if (embedding == null || embedding.Length == 0 || embedding.All(v => v == 0))
                    {
                        _logger.LogWarning(">>> SYNC FAIL (ID={Id}): Embedding service returned empty. Check Gemini API.", item.Id);
                        Interlocked.Increment(ref skippedCount);
                        return;
                    }

                    // Save fresh embedding back to SQL
                    var embeddingJson = JsonSerializer.Serialize(embedding);
                    await _repository.UpdateEmbeddingAsync(item.Id, embeddingJson);

                    // Prepare metadata
                    var metadata = new Dictionary<string, object>
                    {
                        ["question"] = item.Question,
                        ["category"] = "MutualFund",
                        ["source"] = "MutualFundKnowledge"
                    };

                    // Upsert to Qdrant
                    await _qdrantService.UpsertAsync(item.Id, embedding, item.Answer, metadata);
                    Interlocked.Increment(ref syncedCount);

                    _logger.LogInformation(">>> SYNC SUCCESS (ID={Id})", item.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ">>> SYNC EXCEPTION (ID={Id}): {Message}", item.Id, ex.Message);
                    Interlocked.Increment(ref skippedCount);
                }
            });

            _logger.LogInformation(">>> SYNC FINISHED: {Synced} synced, {Skipped} skipped to Qdrant Cloud (768d).", syncedCount, skippedCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error during sync process");
            throw;
        }
    }
}
