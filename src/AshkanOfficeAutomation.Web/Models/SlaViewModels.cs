namespace AshkanOfficeAutomation.Web.Models;
public sealed class SlaItemViewModel
{
    public long LetterId { get; init; }
    public string Number { get; init; } = "";
    public string Subject { get; init; } = "";
    public DateTime? DueAt { get; init; }
    public LetterStatus Status { get; init; }
    public Priority Priority { get; init; }
    public bool IsOverdue => DueAt.HasValue && DueAt.Value < DateTime.Now &&
        Status != LetterStatus.Completed && Status != LetterStatus.Archived;
    public double RemainingHours => DueAt.HasValue ? (DueAt.Value - DateTime.Now).TotalHours : 0;
}
public sealed class SlaDashboardViewModel
{
    public IReadOnlyList<SlaItemViewModel> Items { get; init; } = Array.Empty<SlaItemViewModel>();
    public int Overdue { get; init; }
    public int DueSoon { get; init; }
    public int Healthy { get; init; }
}
