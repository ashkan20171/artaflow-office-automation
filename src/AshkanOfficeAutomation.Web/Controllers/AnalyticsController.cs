using AshkanOfficeAutomation.Web.Data; using AshkanOfficeAutomation.Web.Models; using AshkanOfficeAutomation.Web.Security;
using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Mvc; using Microsoft.EntityFrameworkCore;
namespace AshkanOfficeAutomation.Web.Controllers;
[Authorize(Policy=Permissions.ReportsView)]
public class AnalyticsController(AppDbContext db):Controller {
 public async Task<IActionResult> Index(){
  ViewBag.Total=await db.Letters.CountAsync(); ViewBag.Open=await db.Letters.CountAsync(x=>x.Status==LetterStatus.InWorkflow||x.Status==LetterStatus.Registered);
  ViewBag.Completed=await db.Letters.CountAsync(x=>x.Status==LetterStatus.Completed); ViewBag.Archived=await db.Letters.CountAsync(x=>x.Status==LetterStatus.Archived);
  ViewBag.Urgent=await db.Letters.CountAsync(x=>x.Priority==Priority.Urgent||x.Priority==Priority.Immediate);
  ViewBag.OverdueTasks=await db.WorkItems.CountAsync(x=>x.DueAt<DateTime.Now&&x.Status!=WorkItemStatus.Done&&x.Status!=WorkItemStatus.Cancelled);
  ViewBag.ByType=await db.Letters.GroupBy(x=>x.Type).Select(g=>new KeyValuePair<string,int>(g.Key.ToString(),g.Count())).ToListAsync();
  ViewBag.ByStatus=await db.Letters.GroupBy(x=>x.Status).Select(g=>new KeyValuePair<string,int>(g.Key.ToString(),g.Count())).ToListAsync();
  return View();
 }
}
