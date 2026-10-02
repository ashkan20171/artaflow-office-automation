using AshkanOfficeAutomation.Web.Data; using AshkanOfficeAutomation.Web.Models; using AshkanOfficeAutomation.Web.Security;
using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Identity; using Microsoft.AspNetCore.Mvc; using Microsoft.EntityFrameworkCore;
namespace AshkanOfficeAutomation.Web.Controllers;
[Authorize]
public class DelegationsController(AppDbContext db,UserManager<AppUser> users):Controller{
 string Uid=>users.GetUserId(User)!;
 public async Task<IActionResult> Index(){ViewBag.Users=await db.Users.Where(x=>x.IsActive&&x.Id!=Uid).OrderBy(x=>x.FullName).ToListAsync();return View(await db.Delegations.Where(x=>x.OwnerUserId==Uid).OrderByDescending(x=>x.Id).ToListAsync());}
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Create(string delegateUserId,DateTime from,DateTime to){
  if(to<=from)return BadRequest("بازه زمانی معتبر نیست.");if(!await db.Users.AnyAsync(x=>x.Id==delegateUserId&&x.IsActive))return BadRequest();
  db.Delegations.Add(new Delegation{OwnerUserId=Uid,DelegateUserId=delegateUserId,From=from,To=to});await db.SaveChangesAsync();return RedirectToAction(nameof(Index));
 }
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Disable(long id){var x=await db.Delegations.FirstOrDefaultAsync(x=>x.Id==id&&x.OwnerUserId==Uid);if(x!=null){x.IsActive=false;await db.SaveChangesAsync();}return RedirectToAction(nameof(Index));}
}
