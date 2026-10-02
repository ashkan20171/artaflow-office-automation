using AshkanOfficeAutomation.Web.Data; using AshkanOfficeAutomation.Web.Models; using AshkanOfficeAutomation.Web.Security;
using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Identity; using Microsoft.AspNetCore.Mvc; using Microsoft.EntityFrameworkCore;
namespace AshkanOfficeAutomation.Web.Controllers;
[Authorize] public class MeetingsController(AppDbContext db,UserManager<AppUser> users):Controller {
 string Uid=>users.GetUserId(User)!;
 public async Task<IActionResult> Index(){ViewBag.Users=await db.Users.Where(x=>x.IsActive).OrderBy(x=>x.FullName).ToListAsync();return View(await db.Meetings.AsNoTracking().Where(x=>x.OrganizerId==Uid||User.IsInRole("Administrator")||db.MeetingAttendees.Any(a=>a.MeetingId==x.Id&&a.UserId==Uid)).OrderBy(x=>x.StartsAt).ToListAsync());}
 [Authorize(Policy=Permissions.MeetingsManage),HttpPost,ValidateAntiForgeryToken]
 public async Task<IActionResult> Create(string title,DateTime startsAt,DateTime endsAt,string? location,string? agenda,string[] attendeeIds){
  if(string.IsNullOrWhiteSpace(title)||endsAt<=startsAt)return BadRequest();
  var m=new Meeting{Title=title.Trim(),StartsAt=startsAt,EndsAt=endsAt,Location=location,Agenda=agenda,OrganizerId=Uid};db.Meetings.Add(m);await db.SaveChangesAsync();
  var valid=await db.Users.Where(x=>attendeeIds.Contains(x.Id)&&x.IsActive).Select(x=>x.Id).ToListAsync();
  foreach(var id in valid.Distinct()){db.MeetingAttendees.Add(new MeetingAttendee{MeetingId=m.Id,UserId=id});db.Notifications.Add(new Notification{UserId=id,Title="جلسه جدید",Message=m.Title,Link="/Meetings"});}
  await db.SaveChangesAsync();return RedirectToAction(nameof(Index));
 }
}
