using AshkanOfficeAutomation.Web.Data; using AshkanOfficeAutomation.Web.Models; using AshkanOfficeAutomation.Web.Security;
using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Identity; using Microsoft.AspNetCore.Mvc; using Microsoft.EntityFrameworkCore;
namespace AshkanOfficeAutomation.Web.Controllers;
[Authorize(Policy=Permissions.LettersSearch)]
public class SearchController(AppDbContext db,UserManager<AppUser> users):Controller {
 public async Task<IActionResult> Index([FromQuery]LetterSearchInput input){
  var uid=users.GetUserId(User)!; var q=db.Letters.AsNoTracking().AsQueryable();
  if(!User.IsInRole("Administrator")){
   var owners=await db.Delegations.Where(d=>d.DelegateUserId==uid&&d.IsActive&&d.From<=DateTime.Now&&d.To>=DateTime.Now).Select(d=>d.OwnerUserId).ToListAsync();
   q=q.Where(x=>x.CreatorId==uid||x.Referrals.Any(r=>r.ToUserId==uid||owners.Contains(r.ToUserId)));
  }
  if(!string.IsNullOrWhiteSpace(input.Q)){var t=input.Q.Trim();q=q.Where(x=>x.Subject.Contains(t)||x.Number.Contains(t)||x.Body.Contains(t));}
  if(input.Type.HasValue)q=q.Where(x=>x.Type==input.Type);if(input.Status.HasValue)q=q.Where(x=>x.Status==input.Status);
  if(input.Priority.HasValue)q=q.Where(x=>x.Priority==input.Priority);if(input.From.HasValue)q=q.Where(x=>x.CreatedAt>=input.From);
  if(input.To.HasValue)q=q.Where(x=>x.CreatedAt<input.To.Value.AddDays(1));
  ViewBag.Filter=input; return View(await q.OrderByDescending(x=>x.Id).Take(250).ToListAsync());
 }
}
