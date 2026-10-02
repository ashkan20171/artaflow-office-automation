using AshkanOfficeAutomation.Web.Data;
using AshkanOfficeAutomation.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AshkanOfficeAutomation.Web.Controllers;

[Authorize]
public sealed class NotificationPreferencesController(AppDbContext db, UserManager<AppUser> users) : Controller
{
    public async Task<IActionResult> Index()
    {
        var uid=users.GetUserId(User)!;
        var model=await db.NotificationPreferences.FirstOrDefaultAsync(x=>x.UserId==uid)
            ?? new NotificationPreference { UserId=uid };
        return View(model);
    }

    [HttpPost,ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(NotificationPreference input)
    {
        var uid=users.GetUserId(User)!;
        var model=await db.NotificationPreferences.FirstOrDefaultAsync(x=>x.UserId==uid);
        if(model is null){model=new NotificationPreference{UserId=uid};db.NotificationPreferences.Add(model);}
        model.SlaAlerts=input.SlaAlerts; model.ReferralAlerts=input.ReferralAlerts;
        model.MeetingAlerts=input.MeetingAlerts; model.TaskAlerts=input.TaskAlerts;
        model.SecurityAlerts=input.SecurityAlerts; model.DailyDigest=input.DailyDigest;
        await db.SaveChangesAsync();
        TempData["Info"]="تنظیمات اعلان ذخیره شد.";
        return RedirectToAction(nameof(Index));
    }
}
