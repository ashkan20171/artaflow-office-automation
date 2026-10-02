using AshkanOfficeAutomation.Web.Data; using AshkanOfficeAutomation.Web.Models; using AshkanOfficeAutomation.Web.Services; using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Identity; using Microsoft.AspNetCore.Mvc; using Microsoft.EntityFrameworkCore;
namespace AshkanOfficeAutomation.Web.Controllers;
[Authorize] public sealed class AiCopilotController(IAiCopilotService ai,AppDbContext db,UserManager<AppUser> users):Controller {
 [HttpGet] public async Task<IActionResult> Index()=>View(await PageAsync(new(),null));
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Analyze(AiCopilotInput input,CancellationToken ct){if(!ModelState.IsValid)return View("Index",await PageAsync(input,null));return View("Index",await PageAsync(input,await ai.AnalyzeAsync(input,ct)));}
 async Task<AiCopilotPageViewModel> PageAsync(AiCopilotInput input,AiCopilotResult? result){var uid=users.GetUserId(User)!;var now=DateTime.Now;return new(){Input=input,Result=result,OpenTasks=await db.WorkItems.CountAsync(x=>x.AssigneeId==uid&&x.Status!=WorkItemStatus.Done&&x.Status!=WorkItemStatus.Cancelled),UnreadNotifications=await db.Notifications.CountAsync(x=>x.UserId==uid&&x.ReadAt==null),DueSoon=await db.Letters.CountAsync(x=>x.CreatorId==uid&&x.DueAt!=null&&x.DueAt>=now&&x.DueAt<=now.AddHours(24)&&x.Status!=LetterStatus.Completed&&x.Status!=LetterStatus.Archived)};}
}
