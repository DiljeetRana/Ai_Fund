namespace Ai_Fund.Models;

public class ChatResponse
{
    public string Answer { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public double Confidence { get; set; }
    public string Intent { get; set; } = string.Empty;
}
