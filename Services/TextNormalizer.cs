using System.Text.RegularExpressions;

namespace Ai_Fund.Services;

public static class TextNormalizer
{
    public static string Normalize(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        // Convert to lowercase and trim
        text = text.ToLower().Trim();

        // Remove punctuation (keep only alphanumeric and spaces)
        text = Regex.Replace(text, @"[^a-z0-9\s]", "");

        // Remove extra spaces
        text = Regex.Replace(text, @"\s+", " ");

        return text;
    }
}
