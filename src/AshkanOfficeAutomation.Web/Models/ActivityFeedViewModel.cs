namespace AshkanOfficeAutomation.Web.Models;
public sealed class ActivityFeedViewModel
{
    public IReadOnlyList<AuditEvent> Events { get; init; } = Array.Empty<AuditEvent>();
    public int TodayCount { get; init; }
    public int WeekCount { get; init; }
}
