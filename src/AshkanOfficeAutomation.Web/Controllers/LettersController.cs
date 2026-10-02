using AshkanOfficeAutomation.Web.Data;
using AshkanOfficeAutomation.Web.Models;
using AshkanOfficeAutomation.Web.Security;
using AshkanOfficeAutomation.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace AshkanOfficeAutomation.Web.Controllers;
[Authorize(Policy=Permissions.LettersView)]
public class LettersController(ILetterService service, AppDbContext db, UserManager<AppUser> users, IAuditService audit):Controller {
 private string UserId=>users.GetUserId(User)!;
 public async Task<IActionResult> Index()=>View(await db.Letters.AsNoTracking()
   .Where(x=>x.CreatorId==UserId || User.IsInRole("Administrator") || x.Referrals.Any(r=>r.ToUserId==UserId))
   .OrderByDescending(x=>x.Id).Take(100).ToListAsync());
 public async Task<IActionResult> Details(long id) {
  var letter=await db.Letters.Include(x=>x.Referrals).Include(x=>x.Attachments).AsNoTracking().FirstOrDefaultAsync(x=>x.Id==id);
  if(letter==null)return NotFound();
  if(letter.CreatorId!=UserId&&!User.IsInRole("Administrator")&&!letter.Referrals.Any(x=>x.ToUserId==UserId))return Forbid();
  ViewBag.Users=await db.Users.Where(x=>x.IsActive).OrderBy(x=>x.FullName).ToListAsync();
  ViewBag.Workflow=await db.WorkflowActions.AsNoTracking().Where(x=>x.LetterId==id).OrderByDescending(x=>x.Id).ToListAsync();
  ViewBag.Comments=await db.LetterComments.AsNoTracking().Where(x=>x.LetterId==id).OrderByDescending(x=>x.Id).Take(100).ToListAsync();
  ViewBag.Tags=await db.LetterTags.AsNoTracking().Where(x=>x.LetterId==id).OrderBy(x=>x.Name).ToListAsync();
  ViewBag.Bookmarked=await db.LetterBookmarks.AnyAsync(x=>x.LetterId==id&&x.UserId==UserId);
  return View(letter);
 }
 [Authorize(Policy=Permissions.LettersCreate),HttpGet] public IActionResult Create()=>View(new LetterInput());
 [Authorize(Policy=Permissions.LettersCreate),HttpPost,ValidateAntiForgeryToken]
 public async Task<IActionResult> Create(LetterInput input) {
  if(!ModelState.IsValid)return View(input);
  var model=new Letter{Subject=input.Subject,Type=input.Type,Priority=input.Priority,Sender=input.Sender,
    Receiver=input.Receiver,Body=input.Body,IsConfidential=input.IsConfidential,CreatorId=UserId};
  await service.CreateAsync(model);
  await audit.WriteAsync(UserId,"create","Letter",model.Id.ToString(),HttpContext.Connection.RemoteIpAddress?.ToString());
  return RedirectToAction(nameof(Details),new{id=model.Id});
 }
}
