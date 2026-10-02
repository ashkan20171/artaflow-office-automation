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
public class InboxController(AppDbContext db,UserManager<AppUser> users,IAuditService audit):Controller {
 private string UserId=>users.GetUserId(User)!;
 public async Task<IActionResult> Index()=>View(await db.Referrals.Include(x=>x.Letter).AsNoTracking()
  .Where(x=>x.ToUserId==UserId).OrderByDescending(x=>x.Id).Take(200).ToListAsync());
 [Authorize(Policy=Permissions.LettersRefer),HttpPost,ValidateAntiForgeryToken]
 public async Task<IActionResult> Refer(ReferralInput input) {
  if(!ModelState.IsValid)return BadRequest(ModelState);
  var letter=await db.Letters.Include(x=>x.Referrals).FirstOrDefaultAsync(x=>x.Id==input.LetterId);
  if(letter==null)return NotFound();
  if(letter.CreatorId!=UserId&&!User.IsInRole("Administrator")&&!letter.Referrals.Any(x=>x.ToUserId==UserId))return Forbid();
  if(input.ToUserId==UserId)return BadRequest("ارجاع به خود مجاز نیست.");
  if(!await db.Users.AnyAsync(x=>x.Id==input.ToUserId&&x.IsActive))return BadRequest("کاربر مقصد یافت نشد.");
  db.Referrals.Add(new Referral{LetterId=letter.Id,FromUserId=UserId,ToUserId=input.ToUserId,Note=input.Note});
  db.Notifications.Add(new Notification{UserId=input.ToUserId,Title="ارجاع جدید",Message=$"نامه «{letter.Subject}» به شما ارجاع شد.",Link=$"/Letters/Details/{letter.Id}"});
  letter.Status=LetterStatus.InWorkflow;
  await db.SaveChangesAsync();
  await audit.WriteAsync(UserId,"refer","Letter",letter.Id.ToString(),HttpContext.Connection.RemoteIpAddress?.ToString());
  return RedirectToAction("Details","Letters",new{id=letter.Id});
 }
 [HttpPost,ValidateAntiForgeryToken]
 public async Task<IActionResult> Complete(long id) {
  var referral=await db.Referrals.FirstOrDefaultAsync(x=>x.Id==id&&x.ToUserId==UserId);
  if(referral==null)return NotFound();
  if(referral.DoneAt==null){referral.DoneAt=DateTime.UtcNow;referral.IsDone=true; referral.CompletedAt=DateTime.UtcNow;
  await db.SaveChangesAsync();
   await audit.WriteAsync(UserId,"complete","Referral",id.ToString(),HttpContext.Connection.RemoteIpAddress?.ToString());}
  return RedirectToAction(nameof(Index));
 }
}
