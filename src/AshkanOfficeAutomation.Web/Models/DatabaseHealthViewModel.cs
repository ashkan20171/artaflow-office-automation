namespace AshkanOfficeAutomation.Web.Models;
public sealed class DatabaseHealthViewModel
{
    public bool CanConnect { get; init; }
    public string Provider { get; init; } = "";
    public string Database { get; init; } = "";
    public IReadOnlyList<string> MissingOrganizationColumns { get; init; } = Array.Empty<string>();
    public string Status => !CanConnect ? "Disconnected" :
        MissingOrganizationColumns.Count > 0 ? "Schema mismatch" : "Healthy";
}
