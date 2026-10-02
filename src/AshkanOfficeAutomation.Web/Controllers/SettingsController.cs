using AshkanOfficeAutomation.Web.Data; using AshkanOfficeAutomation.Web.Models; using AshkanOfficeAutomation.Web.Security;
using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Mvc; using Microsoft.EntityFrameworkCore;
namespace AshkanOfficeAutomation.Web.Controllers;
[Authorize(Policy=Permissions.SettingsManage)] public class SettingsController(AppDbContext db):Controller {
 public async Task<IActionResult> Index()=>View(await db.OrganizationSettings.FirstOrDefaultAsync()??new OrganizationSetting());
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Save(OrganizationSetting input){if(input.DefaultSlaHours<1||input.DefaultSlaHours>720)ModelState.AddModelError(nameof(input.DefaultSlaHours),"SLA باید بین ۱ تا ۷۲۰ ساعت باشد.");if(!ModelState.IsValid)return View("Index",input);var x=await db.OrganizationSettings.FirstOrDefaultAsync();if(x==null){input.Id=0;db.OrganizationSettings.Add(input);}else{db.Entry(x).CurrentValues.SetValues(input);x.Id=input.Id==0?x.Id:input.Id;}await db.SaveChangesAsync();return RedirectToAction(nameof(Index));}
}
