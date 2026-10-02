using AshkanOfficeAutomation.Web.Data; using AshkanOfficeAutomation.Web.Models;
using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Identity; using Microsoft.AspNetCore.Mvc; using Microsoft.EntityFrameworkCore;
namespace AshkanOfficeAutomation.Web.Controllers;
[Authorize] public class NotificationsController(AppDbContext db,UserManager<AppUser> users):Controller{
 public async Task<IActionResult> Index(bool unread=false){var id=users.GetUserId(User)!;var q=db.Notifications.AsNoTracking().Where(x=>x.UserId==id);if(unread)q=q.Where(x=>x.ReadAt==null);ViewBag.UnreadOnly=unread;ViewBag.UnreadCount=await db.Notifications.CountAsync(x=>x.UserId==id&&x.ReadAt==null);return View(await q.OrderByDescending(x=>x.Id).Take(100).ToListAsync());}
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Read(long id){var uid=users.GetUserId(User)!;var n=await db.Notifications.FirstOrDefaultAsync(x=>x.Id==id&&x.UserId==uid);if(n!=null){n.ReadAt=DateTime.UtcNow;await db.SaveChangesAsync();}return RedirectToAction(nameof(Index));}
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> ReadAll(){var uid=users.GetUserId(User)!;var items=await db.Notifications.Where(x=>x.UserId==uid&&x.ReadAt==null).ToListAsync();var now=DateTime.UtcNow;foreach(var n in items)n.ReadAt=now;if(items.Count>0)await db.SaveChangesAsync();return RedirectToAction(nameof(Index));}
}
