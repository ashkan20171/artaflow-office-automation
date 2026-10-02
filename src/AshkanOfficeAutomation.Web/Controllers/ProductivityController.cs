using AshkanOfficeAutomation.Web.Data; using AshkanOfficeAutomation.Web.Models; using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Identity; using Microsoft.AspNetCore.Mvc; using Microsoft.EntityFrameworkCore;
namespace AshkanOfficeAutomation.Web.Controllers;
[Authorize] public class ProductivityController(AppDbContext db,UserManager<AppUser> users):Controller {
 public async Task<IActionResult> Index(){var uid=users.GetUserId(User)!;ViewBag.Bookmarks=await db.LetterBookmarks.Where(x=>x.UserId==uid).Join(db.Letters,b=>b.LetterId,l=>l.Id,(b,l)=>l).OrderByDescending(x=>x.Id).ToListAsync();ViewBag.DueLetters=await db.Letters.Where(x=>x.DueAt!=null&&x.DueAt<DateTime.Now.AddDays(7)&&(x.CreatorId==uid||x.Referrals.Any(r=>r.ToUserId==uid))).OrderBy(x=>x.DueAt).ToListAsync();ViewBag.Tasks=await db.WorkItems.Where(x=>x.AssigneeId==uid&&x.Status!=WorkItemStatus.Done&&x.Status!=WorkItemStatus.Cancelled).OrderBy(x=>x.DueAt).Take(20).ToListAsync();return View();}
}
