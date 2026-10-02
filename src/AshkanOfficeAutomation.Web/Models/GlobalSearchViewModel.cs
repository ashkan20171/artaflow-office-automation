namespace AshkanOfficeAutomation.Web.Models;
public sealed class GlobalSearchViewModel
{
    public string Query { get; init; } = "";
    public IReadOnlyList<Letter> Letters { get; init; } = Array.Empty<Letter>();
    public IReadOnlyList<WorkItem> Tasks { get; init; } = Array.Empty<WorkItem>();
    public IReadOnlyList<KnowledgeArticle> Articles { get; init; } = Array.Empty<KnowledgeArticle>();
    public int Total => Letters.Count + Tasks.Count + Articles.Count;
}
