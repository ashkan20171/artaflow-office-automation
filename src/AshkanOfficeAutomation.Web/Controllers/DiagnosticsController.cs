using AshkanOfficeAutomation.Web.Data; using AshkanOfficeAutomation.Web.Security; using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Mvc; using Microsoft.EntityFrameworkCore;
namespace AshkanOfficeAutomation.Web.Controllers;
[Authorize(Policy=Permissions.DiagnosticsView)] public class DiagnosticsController(AppDbContext db,IWebHostEnvironment env):Controller {
 public async Task<IActionResult> Index(){ViewBag.Database=await db.Database.CanConnectAsync();ViewBag.Provider=db.Database.ProviderName;ViewBag.Environment=env.EnvironmentName;ViewBag.Letters=await db.Letters.CountAsync();ViewBag.Users=await db.Users.CountAsync();ViewBag.Pending=await db.Referrals.CountAsync(x=>!x.IsDone);ViewBag.Now=DateTimeOffset.Now;return View();}
}
