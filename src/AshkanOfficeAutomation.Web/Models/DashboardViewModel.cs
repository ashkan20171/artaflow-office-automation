namespace AshkanOfficeAutomation.Web.Models;
public sealed class DashboardViewModel {
 public int TotalLetters {get;init;} public int InWorkflow {get;init;} public int Completed {get;init;}
 public int Archived {get;init;} public int MyOpenTasks {get;init;} public int UnreadNotifications {get;init;}
 public int OverdueTasks {get;init;} public IReadOnlyList<Letter> RecentLetters {get;init;}=Array.Empty<Letter>();
}
