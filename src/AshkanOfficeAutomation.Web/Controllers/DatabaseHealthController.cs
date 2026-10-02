using AshkanOfficeAutomation.Web.Security;
using AshkanOfficeAutomation.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AshkanOfficeAutomation.Web.Controllers;

[Authorize(Policy = Permissions.SettingsManage)]
public sealed class DatabaseHealthController(IDatabaseHealthService health) : Controller
{
    public async Task<IActionResult> Index() => View(await health.CheckAsync());
}
