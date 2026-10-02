using AshkanOfficeAutomation.Web.Data; using AshkanOfficeAutomation.Web.Models; using AshkanOfficeAutomation.Web.Security;
using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Identity; using Microsoft.AspNetCore.Mvc; using Microsoft.EntityFrameworkCore;
namespace AshkanOfficeAutomation.Web.Controllers;
[Authorize]
public class WorkflowController(AppDbContext db,UserManager<AppUser> users):Controller{
 string Uid=>users.GetUserId(User)!;
 bool Can(Letter l)=>User.IsInRole("Administrator")||l.CreatorId==Uid||l.Referrals.Any(r=>r.ToUserId==Uid);
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Action(long letterId,string action,string? note){
  var l=await db.Letters.Include(x=>x.Referrals).FirstOrDefaultAsync(x=>x.Id==letterId);if(l==null)return NotFound();if(!Can(l))return Forbid();
  var allowed=new[]{"submit","approve","reject","archive","reopen"};if(!allowed.Contains(action))return BadRequest();
  if(action=="submit")l.Status=LetterStatus.InWorkflow;
  else if(action=="approve")l.Status=LetterStatus.Completed;
  else if(action=="archive"){if(!User.IsInRole("Administrator")&&!User.HasClaim("permission",Permissions.LettersArchive))return Forbid();l.Status=LetterStatus.Archived;l.ArchivedAt=DateTime.UtcNow;l.ArchiveCode??=$"ARC-{DateTime.Now:yyyy}-{l.Id:000000}";}
  else if(action=="reopen")l.Status=LetterStatus.InWorkflow;
  else if(action=="reject")l.Status=LetterStatus.Draft;
  db.WorkflowActions.Add(new WorkflowAction{LetterId=l.Id,UserId=Uid,Action=action,Note=note});await db.SaveChangesAsync();
  return RedirectToAction("Details","Letters",new{id=l.Id});
 }
}
