using AshkanOfficeAutomation.Web.Data; using AshkanOfficeAutomation.Web.Models; using AshkanOfficeAutomation.Web.Security;
using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Identity; using Microsoft.AspNetCore.Mvc; using Microsoft.EntityFrameworkCore;
namespace AshkanOfficeAutomation.Web.Controllers;
[Authorize] public class KnowledgeController(AppDbContext db,UserManager<AppUser> users):Controller {
 public async Task<IActionResult> Index(string? q){var x=db.KnowledgeArticles.AsNoTracking().Where(a=>a.IsPublished);if(!string.IsNullOrWhiteSpace(q))x=x.Where(a=>a.Title.Contains(q)||a.Content.Contains(q)||(a.Keywords!=null&&a.Keywords.Contains(q)));ViewBag.Q=q;return View(await x.OrderByDescending(a=>a.UpdatedAt).Take(100).ToListAsync());}
 public async Task<IActionResult> Details(long id){var x=await db.KnowledgeArticles.AsNoTracking().FirstOrDefaultAsync(a=>a.Id==id&&a.IsPublished);return x==null?NotFound():View(x);}
 [Authorize(Policy=Permissions.KnowledgeManage),HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Create(string title,string category,string content,string? keywords){if(string.IsNullOrWhiteSpace(title)||string.IsNullOrWhiteSpace(content))return BadRequest();db.KnowledgeArticles.Add(new KnowledgeArticle{Title=title.Trim(),Category=string.IsNullOrWhiteSpace(category)?"عمومی":category.Trim(),Content=content.Trim(),Keywords=keywords,AuthorId=users.GetUserId(User)!});await db.SaveChangesAsync();return RedirectToAction(nameof(Index));}
}
