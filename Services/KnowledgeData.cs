namespace Ai_Fund.Services;

public class KnowledgeData
{
    public int Id { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public string Embedding { get; set; } = string.Empty;
}
