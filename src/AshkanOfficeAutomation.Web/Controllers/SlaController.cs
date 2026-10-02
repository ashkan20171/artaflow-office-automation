using AshkanOfficeAutomation.Web.Security;
using AshkanOfficeAutomation.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AshkanOfficeAutomation.Web.Controllers;

[Authorize(Policy = Permissions.ReportsView)]
public sealed class SlaController(ISlaService sla) : Controller
{
    public async Task<IActionResult> Index() => View(await sla.GetDashboardAsync());

    [HttpPost, ValidateAntiForgeryToken]
    [Authorize(Policy = Permissions.SettingsManage)]
    public async Task<IActionResult> ApplyDefaults(int hours = 48)
    {
        var count = await sla.ApplyDefaultDeadlinesAsync(hours);
        TempData["Info"] = $"برای {count} مکاتبه سررسید پیش‌فرض تنظیم شد.";
        return RedirectToAction(nameof(Index));
    }
}
