using AshkanOfficeAutomation.Web.Data; using AshkanOfficeAutomation.Web.Models; using AshkanOfficeAutomation.Web.Security;
using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Identity; using Microsoft.AspNetCore.Mvc; using Microsoft.EntityFrameworkCore;
namespace AshkanOfficeAutomation.Web.Controllers;
[Authorize]
public class AttachmentsController(AppDbContext db,UserManager<AppUser> users,IWebHostEnvironment env):Controller {
 static readonly HashSet<string> Allowed=new(StringComparer.OrdinalIgnoreCase){".pdf",".doc",".docx",".xls",".xlsx",".png",".jpg",".jpeg",".txt"};
 [Authorize(Policy=Permissions.AttachmentsUpload),HttpPost,ValidateAntiForgeryToken]
 [RequestSizeLimit(15_000_000)]
 public async Task<IActionResult> Upload(long letterId,IFormFile file){
  var uid=users.GetUserId(User)!;var letter=await db.Letters.Include(x=>x.Referrals).FirstOrDefaultAsync(x=>x.Id==letterId);
  if(letter==null)return NotFound();if(letter.CreatorId!=uid&&!User.IsInRole("Administrator")&&!letter.Referrals.Any(x=>x.ToUserId==uid))return Forbid();
  if(file==null||file.Length==0||file.Length>15_000_000)return BadRequest("فایل معتبر نیست.");
  var ext=Path.GetExtension(file.FileName);if(!Allowed.Contains(ext))return BadRequest("نوع فایل مجاز نیست.");
  var stored=$"{Guid.NewGuid():N}{ext}";var folder=Path.Combine(env.ContentRootPath,"App_Data","attachments");Directory.CreateDirectory(folder);
  await using(var fs=System.IO.File.Create(Path.Combine(folder,stored)))await file.CopyToAsync(fs);
  db.Attachments.Add(new Attachment{LetterId=letterId,OriginalName=Path.GetFileName(file.FileName),StoredName=stored,Size=file.Length,ContentType=file.ContentType??"application/octet-stream"});
  await db.SaveChangesAsync();return RedirectToAction("Details","Letters",new{id=letterId});
 }
 public async Task<IActionResult> Download(long id){
  var uid=users.GetUserId(User)!;var a=await db.Attachments.Include(x=>x.Letter).ThenInclude(x=>x!.Referrals).FirstOrDefaultAsync(x=>x.Id==id);
  if(a?.Letter==null)return NotFound();if(a.Letter.CreatorId!=uid&&!User.IsInRole("Administrator")&&!a.Letter.Referrals.Any(x=>x.ToUserId==uid))return Forbid();
  var path=Path.Combine(env.ContentRootPath,"App_Data","attachments",a.StoredName);if(!System.IO.File.Exists(path))return NotFound();
  return PhysicalFile(path,a.ContentType,a.OriginalName);
 }
}
