namespace Ai_Fund.Services;

public static class QueryNormalizer
{
    public static string NormalizeQuery(string query)
    {
        query = query.ToLower().Trim();

        // Remove opinion-seeking phrases
        query = query.Replace("what u think", "");
        query = query.Replace("what you think", "");
        query = query.Replace("your opinion", "");
        query = query.Replace("tell me", "");
        query = query.Replace("can you", "");
        
        // Clean up extra spaces
        while (query.Contains("  "))
        {
            query = query.Replace("  ", " ");
        }

        return query.Trim();
    }

    public static bool IsOpinionQuery(string query)
    {
        query = query.ToLower();
        return query.Contains("think") || 
               query.Contains("suggest") || 
               query.Contains("opinion") ||
               query.Contains("feel");
    }
}
