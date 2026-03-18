namespace Ai_Fund.Services;

public static class ResponseEnhancer
{
    public static string GetContextualPrefix(string query)
    {
        query = query.ToLower();

        // Follow-up confirmations
        if (query.Contains("really") || query.Contains("sure") || query.Contains("certain"))
            return "Yes, ";

        // Safety questions
        if (query.Contains("safe") || query.Contains("risk"))
            return "Yes, ";

        // "Also" questions
        if (query.Contains("also") || query.Contains("too"))
            return "Yes, ";

        // Explanation questions
        if (query.Contains("what") || query.Contains("explain") || query.Contains("tell me"))
            return "";

        // General follow-ups
        if (query.Contains("so") || query.Contains("then"))
            return "So, ";

        return "";
    }

    public static bool IsFollowUpQuestion(string query)
    {
        query = query.ToLower();
        return query.Contains("really") || 
               query.Contains("also") || 
               query.Contains("too") ||
               query.Contains("as well") ||
               query.Contains("what about");
    }
}
