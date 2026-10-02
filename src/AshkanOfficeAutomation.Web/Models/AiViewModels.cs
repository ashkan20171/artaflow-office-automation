using System.ComponentModel.DataAnnotations;
namespace AshkanOfficeAutomation.Web.Models;
public sealed class AiCopilotInput { [Required,StringLength(12000,MinimumLength=10)] public string Text {get;set;}=""; public string Mode {get;set;}="summary"; }
public sealed class AiCopilotResult { public string Title {get;init;}=""; public string Output {get;init;}=""; public string Provider {get;init;}=""; public IReadOnlyList<string> Suggestions {get;init;}=Array.Empty<string>(); }
public sealed class AiCopilotPageViewModel { public AiCopilotInput Input {get;init;}=new(); public AiCopilotResult? Result {get;init;} public int OpenTasks {get;init;} public int UnreadNotifications {get;init;} public int DueSoon {get;init;} }
