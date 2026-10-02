using AshkanOfficeAutomation.Web.Data; using AshkanOfficeAutomation.Web.Models; using AshkanOfficeAutomation.Web.Security;
using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Identity; using Microsoft.AspNetCore.Mvc; using Microsoft.EntityFrameworkCore;
namespace AshkanOfficeAutomation.Web.Controllers;
[Authorize] public class TasksController(AppDbContext db,UserManager<AppUser> users):Controller {
 string Uid=>users.GetUserId(User)!;
 public async Task<IActionResult> Index(){ViewBag.Users=await db.Users.Where(x=>x.IsActive).OrderBy(x=>x.FullName).ToListAsync();return View(await db.WorkItems.AsNoTracking().Where(x=>x.AssigneeId==Uid||x.CreatorId==Uid||User.IsInRole("Administrator")).OrderBy(x=>x.Status).ThenBy(x=>x.DueAt).ToListAsync());}
 [Authorize(Policy=Permissions.TasksManage),HttpPost,ValidateAntiForgeryToken]
 public async Task<IActionResult> Create(string title,string assigneeId,DateTime? dueAt,Priority priority,string? description,long? letterId){
  if(string.IsNullOrWhiteSpace(title)||!await db.Users.AnyAsync(x=>x.Id==assigneeId&&x.IsActive))return BadRequest();
  db.WorkItems.Add(new WorkItem{Title=title.Trim(),AssigneeId=assigneeId,CreatorId=Uid,DueAt=dueAt,Priority=priority,Description=description,LetterId=letterId});await db.SaveChangesAsync();
  db.Notifications.Add(new Notification{UserId=assigneeId,Title="وظیفه جدید",Message=title.Trim(),Link="/Tasks"});await db.SaveChangesAsync();return RedirectToAction(nameof(Index));
 }
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> SetStatus(long id,WorkItemStatus status){
  var x=await db.WorkItems.FirstOrDefaultAsync(x=>x.Id==id&&(x.AssigneeId==Uid||x.CreatorId==Uid||User.IsInRole("Administrator")));if(x==null)return NotFound();x.Status=status;await db.SaveChangesAsync();return RedirectToAction(nameof(Index));
 }
}
