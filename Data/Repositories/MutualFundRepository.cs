using Ai_Fund.Data.Interfaces;
using Ai_Fund.Models;
using System.Data;
using System.Data.SqlClient;

namespace Ai_Fund.Data.Repositories;

public class MutualFundRepository : IMutualFundRepository
{
    private readonly string _connectionString;

    public MutualFundRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new ArgumentNullException("Connection string not found");
    }

    public async Task<List<(int Id, string Question, string Answer, string Embedding)>> GetAllKnowledgeAsync()
    {
        var result = new List<(int, string, string, string)>();

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("SELECT Id, Question, Answer, Embedding FROM MutualFundKnowledge", conn))
            {
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add((
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.IsDBNull(3) ? string.Empty : reader.GetString(3)
                        ));
                    }
                }
            }
        }

        return result;
    }

    public async Task UpdateEmbeddingAsync(int id, string embedding)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("UPDATE MutualFundKnowledge SET Embedding = @Embedding WHERE Id = @Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Embedding", embedding);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task<List<ChatHistory>> GetChatHistoryAsync(string userId, int count = 5)
    {
        var result = new List<ChatHistory>();

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            var query = @"SELECT TOP (@Count) Id, UserId, Role, Message, CreatedDate 
                         FROM ChatHistory 
                         WHERE UserId = @UserId 
                         ORDER BY CreatedDate DESC";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Count", count);

                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new ChatHistory
                        {
                            Id = reader.GetInt32(0),
                            UserId = reader.GetString(1),
                            Role = reader.GetString(2),
                            Message = reader.GetString(3),
                            CreatedDate = reader.GetDateTime(4)
                        });
                    }
                }
            }
        }

        result.Reverse(); // Return in chronological order
        return result;
    }

    public async Task SaveChatHistoryAsync(ChatHistory chatHistory)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            var query = @"INSERT INTO ChatHistory (UserId, Role, Message, CreatedDate) 
                         VALUES (@UserId, @Role, @Message, @CreatedDate)";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@UserId", chatHistory.UserId);
                cmd.Parameters.AddWithValue("@Role", chatHistory.Role);
                cmd.Parameters.AddWithValue("@Message", chatHistory.Message);
                cmd.Parameters.AddWithValue("@CreatedDate", chatHistory.CreatedDate);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task SaveAiLogAsync(AiLog aiLog)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            var query = @"INSERT INTO AiLog (UserId, Query, Response, ConfidenceScore, Intent, Source, CreatedDate) 
                         VALUES (@UserId, @Query, @Response, @ConfidenceScore, @Intent, @Source, @CreatedDate)";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@UserId", aiLog.UserId);
                cmd.Parameters.AddWithValue("@Query", aiLog.Query);
                cmd.Parameters.AddWithValue("@Response", aiLog.Response);
                cmd.Parameters.AddWithValue("@ConfidenceScore", aiLog.ConfidenceScore);
                cmd.Parameters.AddWithValue("@Intent", aiLog.Intent ?? string.Empty);
                cmd.Parameters.AddWithValue("@Source", aiLog.Source ?? string.Empty);
                cmd.Parameters.AddWithValue("@CreatedDate", aiLog.CreatedDate);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
