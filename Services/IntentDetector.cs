namespace Ai_Fund.Services;

public static class IntentDetector
{
    public static string DetectIntent(string query)
    {
        query = query.ToLower();

        // 1. QUESTION should be highest priority
        if (query.Contains("what") || query.Contains("is") || query.Contains("how") || query.Contains("why") || query.Contains("when") || query.Contains("where"))
            return "QUESTION";

        // 2. ADVICE
        if (query.Contains("should") || query.Contains("best") || query.Contains("good") || query.Contains("recommend"))
            return "ADVICE";

        // 3. GREETING (exact match only - LOW priority)
        if (query.Trim() == "hi" || query.Trim() == "hello" || query.Trim() == "hey")
            return "GREETING";

        // 4. CLOSING
        if (query.Contains("thank") || query.Contains("bye"))
            return "CLOSING";

        return "GENERAL";
    }
}
