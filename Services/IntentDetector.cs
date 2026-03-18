namespace Ai_Fund.Services;

public static class IntentDetector
{
    public static string DetectIntent(string query)
    {
        query = query.ToLower();

        if (query.Contains("hi") || query.Contains("hello") || query.Contains("hey"))
            return "GREETING";

        if (query.Contains("what is") || query.Contains("define") || query.Contains("meaning"))
            return "DEFINITION";

        if (query.Contains("how much") || query.Contains("best") || query.Contains("should i") || query.Contains("recommend"))
            return "ADVICE";

        if (query.Contains("thank") || query.Contains("bye"))
            return "CLOSING";

        return "GENERAL";
    }
}
