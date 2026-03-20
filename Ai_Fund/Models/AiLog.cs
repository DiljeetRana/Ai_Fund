namespace Ai_Fund.Models;

public class AiLog
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Query { get; set; } = string.Empty;
    public string Response { get; set; } = string.Empty;
    public double ConfidenceScore { get; set; }
    public string Intent { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}
