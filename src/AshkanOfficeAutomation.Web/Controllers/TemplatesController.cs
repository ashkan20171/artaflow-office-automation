using AshkanOfficeAutomation.Web.Data; using AshkanOfficeAutomation.Web.Models; using AshkanOfficeAutomation.Web.Security;
using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Identity; using Microsoft.AspNetCore.Mvc; using Microsoft.EntityFrameworkCore;
namespace AshkanOfficeAutomation.Web.Controllers;
[Authorize] public class TemplatesController(AppDbContext db,UserManager<AppUser> users):Controller {
 public async Task<IActionResult> Index()=>View(await db.LetterTemplates.AsNoTracking().Where(x=>x.IsActive).OrderBy(x=>x.Name).ToListAsync());
 [Authorize(Policy=Permissions.TemplatesManage),HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Create(string name,string? subjectTemplate,string? bodyTemplate,LetterType type){
  if(string.IsNullOrWhiteSpace(name))return BadRequest();db.LetterTemplates.Add(new LetterTemplate{Name=name.Trim(),SubjectTemplate=subjectTemplate,BodyTemplate=bodyTemplate,Type=type,CreatedById=users.GetUserId(User)!});await db.SaveChangesAsync();return RedirectToAction(nameof(Index));}
 [Authorize(Policy=Permissions.TemplatesManage),HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Disable(long id){var x=await db.LetterTemplates.FindAsync(id);if(x!=null){x.IsActive=false;await db.SaveChangesAsync();}return RedirectToAction(nameof(Index));}
}
