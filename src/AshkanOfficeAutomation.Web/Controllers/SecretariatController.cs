using AshkanOfficeAutomation.Web.Data; using AshkanOfficeAutomation.Web.Models; using AshkanOfficeAutomation.Web.Security;
using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Identity; using Microsoft.AspNetCore.Mvc; using Microsoft.EntityFrameworkCore;
namespace AshkanOfficeAutomation.Web.Controllers;
[Authorize(Policy=Permissions.LettersRegister)]
public class SecretariatController(AppDbContext db,UserManager<AppUser> users):Controller {
 public async Task<IActionResult> Index()=>View(await db.Letters.AsNoTracking().Where(x=>x.Status!=LetterStatus.Draft).OrderByDescending(x=>x.Id).Take(300).ToListAsync());
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Register(long letterId,string? externalNumber,DateTime? externalDate,string? book){
  var l=await db.Letters.FirstOrDefaultAsync(x=>x.Id==letterId);if(l==null)return NotFound();
  if(await db.RegistryEntries.AnyAsync(x=>x.LetterId==letterId))return BadRequest("این نامه قبلاً در دبیرخانه ثبت شده است.");
  db.RegistryEntries.Add(new RegistryEntry{LetterId=letterId,ExternalNumber=externalNumber,ExternalDate=externalDate,Book=string.IsNullOrWhiteSpace(book)?"دفتر مرکزی":book.Trim(),RegisteredById=users.GetUserId(User)!});
  l.Status=LetterStatus.Registered;await db.SaveChangesAsync();return RedirectToAction("Details","Letters",new{id=letterId});
 }
}
