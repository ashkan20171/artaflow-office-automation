using AshkanOfficeAutomation.Web.Data; using AshkanOfficeAutomation.Web.Models;
using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Identity; using Microsoft.AspNetCore.Mvc; using Microsoft.EntityFrameworkCore;
namespace AshkanOfficeAutomation.Web.Controllers;
[Authorize] public class CommandCenterController(AppDbContext db,UserManager<AppUser> users):Controller {
 public async Task<IActionResult> Index(){
  var uid=users.GetUserId(User)!;var now=DateTime.Now;
  ViewBag.Inbox=await db.Referrals.CountAsync(x=>x.ToUserId==uid&&!x.IsDone);
  ViewBag.Tasks=await db.WorkItems.CountAsync(x=>x.AssigneeId==uid&&x.Status!=WorkItemStatus.Done&&x.Status!=WorkItemStatus.Cancelled);
  ViewBag.Overdue=await db.WorkItems.CountAsync(x=>x.AssigneeId==uid&&x.DueAt<now&&x.Status!=WorkItemStatus.Done&&x.Status!=WorkItemStatus.Cancelled);
  ViewBag.Notifications=await db.Notifications.CountAsync(x=>x.UserId==uid&&x.ReadAt==null);
  ViewBag.Meetings=await db.Meetings.CountAsync(x=>x.StartsAt>=now&&x.StartsAt<now.AddDays(7)&&(x.OrganizerId==uid||db.MeetingAttendees.Any(a=>a.MeetingId==x.Id&&a.UserId==uid)));
  ViewBag.Recent=await db.Letters.AsNoTracking().Where(x=>x.CreatorId==uid||x.Referrals.Any(r=>r.ToUserId==uid)).OrderByDescending(x=>x.Id).Take(8).ToListAsync();
  return View();
 }
}
